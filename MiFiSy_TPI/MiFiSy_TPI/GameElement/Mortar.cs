using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiFiSy_TPI.GameElement
{
    internal class Mortar
    {
        private Vector2 _position;
        private Color _color;
        private float _width;
        private float _height;
        private Texture2D _texture;
        private float _angle;

        public Rectangle Rectangle { get => new Rectangle((int)(_position.X * Globals.ScreenWidth), (int)(_position.Y * Globals.ScreenHeight), _texture.Width, _texture.Height); }
        public Vector2 Position { get => _position; set => _position = value; }
        public float Width { get => _width; set => _width = value; }
        public float Angle { get => _angle; set => _angle = value; }

        public Mortar(Vector2 position, float width, float height, Color color)
        {
            _position = position;
            _color = color;
            _width = width;
            _height = height;
            _angle = Globals.RandomFloat(-10, 10);

            // Converti l'angle en radians
            if (_angle >= 0)
            {
                _angle = MathHelper.ToRadians(_angle);
            }
            else
            {
                _angle = -MathHelper.ToRadians(Math.Abs(_angle));
            }
            SetTexture();
        }

        /// <summary>
        /// Crée la texture du rectangle du bouton avec ses dimensions et sa couleur
        /// </summary>
        public void SetTexture()
        {
            int width = (int)(_width * Globals.ScreenWidth);
            int height = (int)(_height * Globals.ScreenHeight);
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
            Globals.SpriteBatch.Draw(_texture, Rectangle, null ,_color, _angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
