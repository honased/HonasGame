using HonasGame.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.Tiled.Entities
{
    public class TiledTilesEntity : Entity
    {
        private const uint FLIPPED_HORIZONTALLY = 0x80000000;
        private const uint FLIPPED_VERTICALLY = 0x40000000;
        private const uint FLIPPED_DIAGONALLY = 0x20000000;
        private const uint FLIPPED_HEXAGONAL = 0x10000000;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        private uint[] _tiles;
        private List<TiledMap.Tileset> _tilesets;

        public TiledTilesEntity(int w, int h, int tw, int th, uint[] tiles)
        {
            Width = w;
            Height = h;
            TileWidth = tw;
            TileHeight = th;
            _tiles = tiles;
        }

        internal void SetTilesets(List<TiledMap.Tileset> tilesets)
        {
            _tilesets = tilesets;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            string cachedTexStr = "";
            Texture2D cachedTex = null;

            int oX = (int)Camera.Position.X / TileWidth - 2;
            int oY = (int)Camera.Position.Y / TileHeight - 2;
            oX = MathHelper.Clamp(oX, 0, Width);
            oY = MathHelper.Clamp(oY, 0, Height);

            int xStop = (int)Camera.CameraSize.X / TileWidth + 4;
            int yStop = (int)Camera.CameraSize.Y / TileHeight + 4;
            xStop = MathHelper.Clamp(oX + xStop, 0, Width);
            yStop = MathHelper.Clamp(oY + yStop, 0, Height);

            for (int y = oY; y < yStop; y++)
            {
                for (int x = oX; x < xStop; x++)
                {
                    int j = (y * Width) + x;
                    uint tile = _tiles[j];

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
                        if (_set.FirstGid <= tile)
                        {
                            tile -= (uint)_set.FirstGid;

                            TiledTileset actualSet = Assets.AssetLibrary.GetAsset<TiledTileset>(_set.Source);

                            if (cachedTexStr != actualSet.Image)
                            {
                                cachedTexStr = actualSet.Image;
                                cachedTex = Assets.AssetLibrary.GetAsset<Texture2D>(actualSet.Image);
                            }

                            Vector2 pos = new Vector2(x * TileWidth, y * TileHeight);
                            Vector2 sourcePos = actualSet.GetTileLocation(tile);
                            Rectangle sourceRect = new Rectangle((int)sourcePos.X, (int)sourcePos.Y, TileWidth, TileHeight);
                            spriteBatch.Draw(cachedTex, pos, sourceRect, Color.White, 0.0f, Vector2.Zero, Vector2.One, effects, 0.0f);
                            break;
                        }

                    }
                }
            }

            base.Draw(gameTime, spriteBatch);
        }

        protected override void Cleanup()
        {

        }
    }
}
