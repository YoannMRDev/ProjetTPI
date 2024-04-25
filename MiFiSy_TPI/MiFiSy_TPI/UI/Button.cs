﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;

namespace MiFiSy_TPI.UI
{
    internal class Button
    {
        private Vector2 _position;
        private Vector2 _textPosition;
        private Texture2D _texture;
        private string _text;
        private float _widthRectangle;
        private float _heightRectangle;
        private Color _backgroundColor;
        private Color _textColor;
        private float _padding;
        private float _scale;
        private string _action;

        public Rectangle Rectangle { get => new Rectangle((int)(_position.X * Globals.ScreenWidth), (int)(_position.Y * Globals.ScreenHeight), _texture.Width, _texture.Height); }

        public Button(Vector2 position, float width, float height, string text, Color backgroundColor, Color textColor, string action, float padding = 0.2f)
        {
            _position = position;
            _widthRectangle = width;
            _heightRectangle = height;
            _text = text;
            _backgroundColor = backgroundColor;
            _textColor = textColor;
            _padding = padding;
            _action = action;
            _scale = 1;

            SetTexture();
            SetTextPositionAndScale();
        }

        /// <summary>
        /// Crée la texture du rectangle du bouton avec ses dimensions et sa couleur
        /// </summary>
        public void SetTexture()
        {
            int width = (int)(_widthRectangle * Globals.ScreenWidth);
            int height = (int)(_heightRectangle * Globals.ScreenHeight);
            _texture = new Texture2D(Globals.GraphicsDevice, width, height);
            Color[] colorData = new Color[width * height];
            for (int i = 0; i < colorData.Length; ++i)
            {
                colorData[i] = _backgroundColor;
            }
            _texture.SetData(colorData);
        }

        /// <summary>
        /// Calcule la position et la taille du text par rapport à la largeur du rectangle qu'il contient
        /// </summary>
        public void SetTextPositionAndScale()
        {
            if (Globals.FontButton.MeasureString(_text).X != 0)
            {
                // Calcul du facteur d'échelle pour le texte
                float scaleX = (Rectangle.Width * (1 - _padding)) / Globals.FontButton.MeasureString(_text).X;
                float scaleY = (Rectangle.Height * (1 - _padding)) / Globals.FontButton.MeasureString(_text).Y;
                _scale = Math.Min(scaleX, scaleY);
            }
            else
            {
                _scale = 1;
            }

            _textPosition.X = Rectangle.X + (Rectangle.Width - Globals.FontButton.MeasureString(_text).X * _scale) / 2;
            _textPosition.Y = Rectangle.Y + (Rectangle.Height - Globals.FontButton.MeasureString(_text).Y * _scale) / 2;
        }

        public void Update()
        {
            switch (_action)
            {
                case "goBack":
                    if (InputManager.HasClicked && Rectangle.Contains(InputManager.MousePosition))
                    {
                        Globals.ActualPage = Globals.AllPage.Accueil;
                    }
                    break;
                case "save":
                    // Sauvegarder la séquence
                    break;
                default:
                    break;
            }
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(_texture, Rectangle, Color.White);
            Globals.SpriteBatch.DrawString(Globals.FontButton, _text, _textPosition, _textColor, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }
    }
}