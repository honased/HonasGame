/// Aseprite file format documentation: https://github.com/aseprite/aseprite/blob/main/docs/ase-file-specs.md

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace HonasGame.Assets
{
    public class Aseprite
    {
        private enum ColorFormat
        {
            RGBA,
            Grayscale,
            Indexed
        }

        public struct Frame
        {
            public float duration;
            public Color[] pixels;
        }

        public struct Layer
        {
            public byte alpha;
        }

        private BinaryReader _br;
        private ColorFormat _colorFormat;
        private Color[] _palette;

        public Frame[] Frames { get; private set; }
        public List<Layer> Layers { get; private set; }
        public int FrameCount => Frames.Length;
        public int Width { get; private set; }
        public int Height { get; private set; }

        private byte Byte() => _br.ReadByte();
        private ushort Word() => _br.ReadUInt16();
        private short Short() => _br.ReadInt16();
        private uint DWord() => _br.ReadUInt32();
        private int Long() => _br.ReadInt32();
        private float Fixed() => _br.ReadSingle();
        private byte[] Bytes(uint n)
        {
            byte[] bytes = new byte[n];
            for(uint i = 0; i < n; i++)
            {
                bytes[i] = Byte();
            }
            return bytes;
        }
        private string String() => _br.ReadChars(Word()).ToString();
        private Color ReadColor()
        {
            switch(_colorFormat)
            {
                case ColorFormat.RGBA:
                    return Color.FromNonPremultiplied(Byte(), Byte(), Byte(), Byte());

                case ColorFormat.Grayscale:
                    int c = Byte();
                    return Color.FromNonPremultiplied(c, c, c, Byte());

                case ColorFormat.Indexed:
                    return _palette[Byte()];

                default:
                    throw new Exception("Error! Unknown Color Format");
            }
        }

        private void Seek(long position)
        {
            _br.BaseStream.Position = position;
        }

        private long Position()
        {
            return _br.BaseStream.Position;
        }

        public Aseprite(string file)
        {
            _br = new BinaryReader(File.OpenRead(file));
            Layers = new List<Layer>();

            DWord(); // File Size
            if (Word() != 0xA5E0) throw new Exception("Error! Not an Aseprite File"); // Magic Number
            Frames = new Frame[Word()];

            Width = Word();
            Height = Word();

            for (int i = 0; i < FrameCount; i++) Frames[i].pixels = new Color[Width * Height];

            switch(Word())
            {
                case 32: _colorFormat = ColorFormat.RGBA;       break;
                case 16: _colorFormat = ColorFormat.Grayscale;  break;
                case 8:  _colorFormat = ColorFormat.Indexed;    break;
                default: throw new Exception("Unknown color format");
            }
            DWord(); // Valid layer opacity
            Word(); // Deprecated frame speed
            DWord(); // Set be 0
            DWord(); // Set be 0
            Byte(); // Palette entry which represents transparent color
            Bytes(3); // Ignore these bytes
            Word(); // Number of colors (0 means 256 for old sprites)
            Byte(); // Pixel width
            Byte(); // Pixel Height
            Short(); // X position of the grid
            Short(); // Y position of the grid
            Word(); // Grid Width
            Word(); // Grid Height
            Bytes(84); // For future (set to zero)
            ReadFrames();

            _br.Close();
        }

        private void ReadFrames()
        {
            for(int i = 0; i < FrameCount; i++)
            {
                uint numChunks;

                DWord(); // Bytes in this frame
                if (Word() != 0xF1FA) throw new Exception("Error! Not a frame!");
                numChunks = Word();
                Frames[i].duration = Word() / 1000.0f;
                Bytes(2); // For future (set to zero)
                uint newNumChunks = DWord();
                if (newNumChunks > 0) numChunks = newNumChunks;

                while(numChunks-- > 0) ReadChunk(ref Frames[i]);
            }
        }

        private void ReadChunk(ref Frame frame)
        {
            long seekLocation = Position();
            seekLocation += DWord();
            ushort chunkType = Word();

            switch(chunkType)
            {
                // Old palette chunk
                case 0x0004:
                    // Ignore for now
                    break;

                // Old palette chunk
                case 0x0011:
                    // Ignore for now
                    break;

                // Layer Chunk
                case 0x2004:
                    Layer l = new Layer();

                    Word(); // Flags
                    Word(); // Layer Type
                    Word(); // Layer child level
                    Word(); // width
                    Word(); // height
                    Word(); // Blend mode
                    l.alpha = Byte();
                    Bytes(3); // For future (set to zero)
                    String(); // Name

                    Layers.Add(l);
                    break;

                // Cel Chunk
                case 0x2005:
                    ushort layerIndex = Word();
                    short xPos = Short(), yPos = Short();
                    byte opacity = Byte();
                    ushort celType = Word();
                    Bytes(7); // For Future
                    switch(celType)
                    {
                        case 2:
                            DeflateStream ds;
                            ushort width = Word(), height = Word();
                            byte[] data = new byte[width * height * 4];
                            Bytes(2); // For deflate stream
                            ds = new DeflateStream(_br.BaseStream, CompressionMode.Decompress);
                            ds.Read(data, 0, width * height * 4);
                            int colorIndex = 0;
                            
                            for(short y = yPos; y < yPos + height; y++ )
                            {
                                for(short x = xPos; x < xPos + width; x++)
                                {
                                    int index = (y * Width) + x;
                                    ApplyColor(ref frame, index, Color.FromNonPremultiplied(data[colorIndex], data[colorIndex + 1], data[colorIndex + 2], data[colorIndex + 3]), Layers[layerIndex].alpha);
                                    colorIndex += 4;
                                }
                            }
                            break;
                    }
                    break;

                case 0x2007:
                    // Ignore for now
                    break;

                // Palette Chunk
                case 0x2019:
                    _palette = new Color[DWord()];
                    uint __startIndex = DWord(), __endIndex = DWord();
                    Bytes(8); // For future (set to zero)
                    for(uint i = __startIndex; i <= __endIndex; i++)
                    {
                        bool hasName = (Word() & 1) > 0;
                        _palette[i] = Color.FromNonPremultiplied(Byte(), Byte(), Byte(), Byte());
                        if (hasName) String();
                    }
                    break;
            }

            Seek(seekLocation);
        }

        private void ApplyColor(ref Frame frame, int index, Color source, byte alpha)
        {
            Color destination = frame.pixels[index];

            if(source.A != 0)
            {
                Color finalColor = Color.Beige;
                if (destination.A == 0) finalColor = source;
                else
                {
                    var sa = MUL_UN8(source.A, alpha);
                    var ra = destination.A + sa - MUL_UN8(destination.A, sa);

                    finalColor.R = (byte)(destination.R + (source.R - destination.R) * sa / ra);
                    finalColor.G = (byte)(destination.G + (source.G - destination.G) * sa / ra);
                    finalColor.B = (byte)(destination.B + (source.B - destination.B) * sa / ra);
                    finalColor.A = (byte)ra;
                }

                frame.pixels[index] = finalColor;
            }
        }

        private int MUL_UN8(int a, int b)
        {
            var t = (a * b) + 0x80;
            return (((t >> 8) + t) >> 8);
        }
    }
}
