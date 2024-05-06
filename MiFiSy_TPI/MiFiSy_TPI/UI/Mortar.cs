using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiFiSy_TPI.UI
{
    internal class Mortar
    {
        private Vector2 _position;
        private Color _color;
        private float _width;
        private float _height;
        private Texture2D _texture;
        private float _angle;

        public Rectangle Rectangle { get => new Rectangle((int)(Position.X * Globals.ScreenWidth), (int)(Position.Y * Globals.ScreenHeight), _texture.Width, _texture.Height); }
        public Vector2 Position { get => _position; set => _position = value; }
        public float Width { get => _width; set => _width = value; }
        public float Angle { get => _angle; set => _angle = value; }
        public float Height { get => _height; set => _height = value; }

        public Mortar(Vector2 position, float width, float height, float angle, Color color)
        {
            Position = position;
            _color = color;
            Width = width;
            Height = height;
            Angle = Globals.RandomFloat(-angle, angle);

            // Converti l'angle en radians
            if (Angle >= 0)
            {
                Angle = MathHelper.ToRadians(Angle);
            }
            else
            {
                Angle = -MathHelper.ToRadians(Math.Abs(Angle));
            }
            SetTexture();
        }

        /// <summary>
        /// Crée la texture du rectangle du bouton avec ses dimensions et sa couleur
        /// </summary>
        public void SetTexture()
        {
            int width = (int)(Width * Globals.ScreenWidth);
            int height = (int)(Height * Globals.ScreenHeight);
            _texture = new Texture2D(Globals.GraphicsDevice, width, height);
            Color[] colorData = new Color[width * height];
            for (int i = 0; i < colorData.Length; ++i)
            {
                colorData[i] = _color;
            }
            _texture.SetData(colorData);
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(_texture, Rectangle, null, _color, Angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
