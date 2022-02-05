using HonasGame.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.ECS.Components
{
    public class SpriteRenderer : Component
    {
        private Sprite _sprite;
        private string _animationName;
        private SpriteAnimation _animation;
        private Transform2D _position;

        private double _timer;
        private int _frameIndex;

        public Vector2 Scale { get; set; } = Vector2.One;
        public Vector2 Origin { get; set; }
        public Color Color { get; set; } = Color.White;
        public float Rotation { get; set; }

        public Sprite Sprite
        {
            get => _sprite;
            set
            {
                _sprite = value;
                ResetRenderData();
            }
        }

        public string Animation
        {
            get => _animationName;
            set
            {
                _animationName = value;
                ResetRenderData();
                if(!_sprite.Animations.TryGetValue(_animationName, out _animation))
                {
                    _animation = new SpriteAnimation() { Frames = new Rectangle[] { _sprite.Texture.Bounds }, FrameTime = 0.0 };
                }
            }
        }

        public int FrameIndex
        {
            get => _frameIndex;
            set
            {
                _frameIndex = value % _animation.Frames.Length;
                _timer = 0.0;
            }
        }

        public SpriteRenderer(Entity parent) : base(parent)
        {
            ResetRenderData();
            _position = Parent.GetComponent<Transform2D>();
        }

        public override void Update(GameTime gameTime)
        {
            if(_animation.FrameTime > 0.0)
            {
                _timer += gameTime.ElapsedGameTime.TotalSeconds;

                if(_timer > _animation.FrameTime)
                {
                    _timer -= _animation.FrameTime;
                    _frameIndex = (_frameIndex + 1) % _animation.Frames.Length;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sprite.Texture, _position.Position, _animation.Frames[_frameIndex], Color, Rotation, Origin, Scale, SpriteEffects.None, 0.0f);
        }

        private void ResetRenderData()
        {
            _timer = 0.0;
            _frameIndex = 0;
        }

        public void CenterOrigin()
        {
            Origin = new Vector2(_animation.Frames[_frameIndex].Width / 2.0f, _animation.Frames[_frameIndex].Height / 2.0f);
        }
    }
}
