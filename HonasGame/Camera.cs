using Microsoft.Xna.Framework;
using System;

namespace HonasGame
{
    public static class Camera
    {
        private static Vector2 _position;
        public static Vector2 Position
        {
            get => _position;
            set
            {
                _position = new Vector2(MathHelper.Clamp(value.X, Bounds.Left, Bounds.Right - CameraSize.X),
                                        MathHelper.Clamp(value.Y, Bounds.Top, Bounds.Bottom - CameraSize.Y));
            }
        }
        public static Vector2 CameraSize { get; set; }

        public static Rectangle Bounds { get; set; }

        private static float _shakeAmount;
        private static double _shakeTimer;
        private static Random _random = new Random();

        public static Matrix GetMatrix(Vector2 WindowSize)
        {
            float shakeX = (float)(_random.NextDouble() * 2 - 1) * _shakeAmount;
            float shakeY = (float)(_random.NextDouble() * 2 - 1) * _shakeAmount;
            Vector2 clampedPosition = new Vector2(MathHelper.Clamp(Position.X + shakeX, Bounds.Left, Bounds.Right - CameraSize.X),
                                                  MathHelper.Clamp(Position.Y + shakeY, Bounds.Top, Bounds.Bottom - CameraSize.Y));
            var mat = Matrix.CreateTranslation(new Vector3(-clampedPosition, 0.0f));
            mat *= Matrix.CreateScale(new Vector3(WindowSize / CameraSize, 1.0f));

            return mat;
        }

        internal static void Update(GameTime gameTime)
        {
            if(_shakeTimer > 0.0)
            {
                _shakeTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _shakeAmount = 0.0f;
            }
        }

        public static void ShakeScreen(float amount, double time)
        {
            _shakeAmount = amount;
            _shakeTimer = time;
        }
    }
}
