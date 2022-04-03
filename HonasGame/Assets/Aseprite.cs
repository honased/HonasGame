/// Aseprite file format documentation: https://github.com/aseprite/aseprite/blob/main/docs/ase-file-specs.md

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
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
            public List<Color[]> layers;
        }

        private BinaryReader _br;
        private ColorFormat _colorFormat;
        private Color[] _palette;

        public Frame[] Frames { get; private set; }
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
            DWord(); // File Size
            if (Word() != 0xA5E0) throw new Exception("Error! Not an Aseprite File"); // Magic Number
            Frames = new Frame[Word()];

            // Initialize Frames
            for (int i = 0; i < FrameCount; i++) Frames[i].layers = new List<Color[]>();

            Width = Word();
            Height = Word();
            _colorFormat = (ColorFormat)Word();
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
        }

        private void ReadFrames()
        {
            for(int i = 0; i < FrameCount; i++)
            {
                uint numChunks;

                Frame f = new Frame();
                f.layers = new List<Color[]>();

                DWord(); // Bytes in this frame
                if (Word() != 0xF1FA) throw new Exception("Error! Not a frame!");
                numChunks = Word();
                f.duration = Word() / 1000.0f;
                Bytes(2); // For future (set to zero)
                uint newNumChunks = DWord();
                if (newNumChunks > 0) numChunks = newNumChunks;

                while(numChunks-- > 0) ReadChunk(ref f);

                Frames[i] = f;
            }
        }

        private void ReadChunk(ref Frame frame)
        {
            long seekLocation = DWord();
            ushort chunkType = Word();
            seekLocation += Position();

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
                    
                    // Ignore rest for now
                    break;
            }

            Seek(seekLocation);
        }
    }
}
