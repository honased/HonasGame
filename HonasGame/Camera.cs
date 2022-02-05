using Microsoft.Xna.Framework;

namespace HonasGame
{
    public static class Camera
    {
        public static Vector2 Position { get; set; }
        public static Vector2 Size { get; set; }
        public static Vector2 Zoom { get; set; }

        public static Matrix GetMatrix()
        {
            var mat = Matrix.CreateTranslation(new Vector3(-Position, 0.0f));

            return mat;
        }
    }
}
