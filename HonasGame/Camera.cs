using Microsoft.Xna.Framework;

namespace HonasGame
{
    public static class Camera
    {
        public static Vector2 Position { get; set; }
        public static Vector2 CameraSize { get; set; }

        public static Rectangle Bounds { get; set; }

        public static Matrix GetMatrix(Vector2 WindowSize)
        {
            Vector2 clampedPosition = new Vector2(MathHelper.Clamp(Position.X, Bounds.Left, Bounds.Right - CameraSize.X),
                                                  MathHelper.Clamp(Position.Y, Bounds.Top, Bounds.Bottom - CameraSize.Y));
            var mat = Matrix.CreateTranslation(new Vector3(-clampedPosition, 0.0f));
            mat *= Matrix.CreateScale(new Vector3(WindowSize / CameraSize, 1.0f));

            return mat;
        }
    }
}
