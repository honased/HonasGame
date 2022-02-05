using HonasGame.JSON;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HonasGame.Tiled
{
    public class TiledMap
    {
        private const uint FLIPPED_HORIZONTALLY = 0x80000000;
        private const uint FLIPPED_VERTICALLY   = 0x40000000;
        private const uint FLIPPED_DIAGONALLY   = 0x20000000;
        private const uint FLIPPED_HEXAGONAL    = 0x10000000;

        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        private int primitives;

        private class Tileset
        {
            public int FirstGid { get; private set; }
            public string Source { get; private set; }

            public Tileset(JObject jObj)
            {
                FirstGid = (int)jObj.GetValue<double>("firstgid");
                Source = Path.GetFileNameWithoutExtension(jObj.GetValue<string>("source"));
            }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public List<TiledLayer> Layers { get; private set; }
        private List<Tileset> _tilesets;

        public TiledMap(JObject jObj)
        {
            Width       = (int)jObj.GetValue<double>("width");
            Height      = (int)jObj.GetValue<double>("height");
            TileWidth   = (int)jObj.GetValue<double>("tilewidth");
            TileHeight  = (int)jObj.GetValue<double>("tileheight");

            Layers = new List<TiledLayer>();

            JArray arr = jObj.GetValue<JArray>("layers");

            for(int i = 0; i < arr.Count; i++)
            {
                JObject layer = arr.GetValue<JObject>(i);
                switch(layer.GetValue<string>("type"))
                {
                    case "tilelayer":
                        Layers.Add(new TiledTileLayer(layer));
                        break;

                    case "objectgroup":
                        Layers.Add(new TiledObjectLayer(layer));
                        break;
                }
            }

            _tilesets = new List<Tileset>();
            arr = jObj.GetValue<JArray>("tilesets");
            for(int i = 0; i < arr.Count; i++)
            {
                _tilesets.Add(new Tileset(arr.GetValue<JObject>(i)));
            }
        }

        public void GenerateVBO(GraphicsDevice graphicsDevice)
        {
            List<VertexPositionTexture> vertices = new List<VertexPositionTexture>();
            List<short> indices = new List<short>();

            short vertexCount = 0;

            for (int i = 0; i < Layers.Count; i++)
            {
                if (Layers[i] is TiledTileLayer tileLayer)
                {
                    for (int j = 0; j < tileLayer.Width * tileLayer.Height; j++)
                    {
                        SpriteEffects effects = SpriteEffects.None;
                        uint tile = tileLayer.Data[j];
                        bool flippedVertically = (tile & FLIPPED_VERTICALLY) > 0;
                        bool flippedHorizontally = (tile & FLIPPED_HORIZONTALLY) > 0;
                        tile &= ~(FLIPPED_HORIZONTALLY | FLIPPED_VERTICALLY | FLIPPED_DIAGONALLY | FLIPPED_HEXAGONAL);

                        if (flippedHorizontally) effects |= SpriteEffects.FlipHorizontally;
                        if (flippedVertically) effects |= SpriteEffects.FlipVertically;

                        // Find Tileset
                        for (int k = _tilesets.Count - 1; k >= 0; k--)
                        {
                            var _set = _tilesets[k];
                            if (_set.FirstGid <= tile)
                            {
                                tile -= (uint)_set.FirstGid;

                                TiledTileset actualSet = Assets.AssetLibrary.GetAsset<TiledTileset>(_set.Source);
                                Texture2D tex = Assets.AssetLibrary.GetAsset<Texture2D>(actualSet.Image);

                                Vector2 pos = new Vector2((j % Width) * TileWidth, (j / Width) * TileHeight);
                                Vector2 sourcePos = actualSet.GetTileLocation(tile);
                                Vector2 texelSize = new Vector2(1.0f / tex.Width, 1.0f / tex.Height);
                                Vector2 tl = sourcePos * texelSize;
                                Vector2 tr = (sourcePos + new Vector2(TileWidth, 0)) * texelSize;
                                Vector2 bl = (sourcePos + new Vector2(0, TileHeight)) * texelSize;
                                Vector2 br = (sourcePos + new Vector2(TileWidth, TileHeight)) * texelSize;

                                vertices.Add(new VertexPositionTexture(new Vector3(pos, 0.0f), tl));
                                vertices.Add(new VertexPositionTexture(new Vector3(pos + new Vector2(TileWidth, 0), 0.0f), tr));
                                vertices.Add(new VertexPositionTexture(new Vector3(pos + new Vector2(0, TileHeight), 0.0f), bl));
                                vertices.Add(new VertexPositionTexture(new Vector3(pos + new Vector2(TileWidth, TileHeight), 0.0f), br));

                                indices.Add((short)(vertexCount + 2));
                                indices.Add((short)(vertexCount + 1));
                                indices.Add((short)(vertexCount + 0));
                                indices.Add((short)(vertexCount + 2));
                                indices.Add((short)(vertexCount + 3));
                                indices.Add((short)(vertexCount + 1));

                                vertexCount += 4;

                                break;
                            }

                        }
                    }
                }
            }

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), vertices.Count, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices.ToArray());

            indexBuffer = new IndexBuffer(graphicsDevice, typeof(short), indices.Count, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices.ToArray());

            primitives = indices.Count / 3;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for(int i = 0; i < Layers.Count; i++)
            {
                if(Layers[i] is TiledTileLayer tileLayer)
                {
                    string cachedTexStr = "";
                    Texture2D cachedTex = null;

                    for(int j = 0; j < tileLayer.Width * tileLayer.Height; j++)
                    {
                        uint tile = tileLayer.Data[j];

                        if (tile == 0) continue;

                        SpriteEffects effects = SpriteEffects.None;
                        bool flippedVertically = (tile & FLIPPED_VERTICALLY) > 0;
                        bool flippedHorizontally = (tile & FLIPPED_HORIZONTALLY) > 0;
                        tile &= ~(FLIPPED_HORIZONTALLY | FLIPPED_VERTICALLY | FLIPPED_DIAGONALLY | FLIPPED_HEXAGONAL);

                        if (flippedHorizontally) effects |= SpriteEffects.FlipHorizontally;
                        if (flippedVertically) effects |= SpriteEffects.FlipVertically;

                        // Find Tileset
                        for (int k = _tilesets.Count - 1; k >= 0; k--)
                        {
                            var _set = _tilesets[k];
                            if(_set.FirstGid <= tile)
                            {
                                tile -= (uint)_set.FirstGid;

                                TiledTileset actualSet = Assets.AssetLibrary.GetAsset<TiledTileset>(_set.Source);

                                if(cachedTexStr != actualSet.Image)
                                {
                                    cachedTexStr = actualSet.Image;
                                    cachedTex = Assets.AssetLibrary.GetAsset<Texture2D>(actualSet.Image);
                                }

                                Vector2 pos = new Vector2((j % Width) * TileWidth, (j / Width) * TileHeight);
                                Vector2 sourcePos = actualSet.GetTileLocation(tile);
                                Rectangle sourceRect = new Rectangle((int)sourcePos.X, (int)sourcePos.Y, TileWidth, TileHeight);
                                spriteBatch.Draw(cachedTex, pos, sourceRect, Color.White, 0.0f, Vector2.Zero, Vector2.One, effects, 0.0f);
                                break;
                            }

                        }
                    }
                }
            }
        }
    }
}
