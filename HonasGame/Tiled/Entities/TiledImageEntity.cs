using HonasGame.Assets;
using HonasGame.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.Tiled.Entities
{
    public class TiledImageEntity : Entity
    {
        private Texture2D _texture;
        private Vector2 _offset;
        private Vector2 _parallax;
        private int _opacity;

        public TiledImageEntity(TiledImageLayer layer)
        {
            _texture = AssetLibrary.GetAsset<Texture2D>(layer.Image);
            _offset = new Vector2(layer.OffsetX, layer.OffsetY);
            _parallax = new Vector2(layer.ParallaxX, layer.ParallaxY);
            _opacity = (int)(layer.Opacity * 255);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int offsetIndex = (int)(Math.Round(Camera.Position.X) / (_texture.Width / (1 - _parallax.X)));
            spriteBatch.Draw(_texture, Camera.Position * _parallax + ( _offset * (Vector2.One - _parallax) ) + (Vector2.UnitX * _texture.Width * offsetIndex), Color.FromNonPremultiplied(255, 255, 255, _opacity));
            spriteBatch.Draw(_texture, Camera.Position * _parallax + ( _offset * (Vector2.One - _parallax) ) + (Vector2.UnitX * _texture.Width * (offsetIndex + 1)), Color.FromNonPremultiplied(255, 255, 255, _opacity));


            base.Draw(gameTime, spriteBatch);
        }

        protected override void Cleanup()
        {

        }
    }
}
