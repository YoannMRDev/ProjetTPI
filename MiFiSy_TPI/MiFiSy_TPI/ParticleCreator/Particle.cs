using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiFiSy_TPI.ParticleCreator.Structure;
using System;
/*
 * Auteur : Yoann Meier
 * Date : 06/05/2024
 * Projet : Projet TPI, application de simulation de feux d'artifices en 2D
 * Description de la page : Class d'une particule (vient de : https://www.youtube.com/watch?v=-4_kj_gyWRY)
 */
namespace MiFiSy_TPI.ParticleCreator
{
    internal class Particle
    {
        private ParticleData _data;
        private Vector2 _position;
        private float _lifespanLeft;
        private float _lifespanAmount;
        private Color _color;
        private float _opacity;
        public bool isFinished = false;
        private float _scale;
        private Vector2 _origin;
        private Vector2 _direction;

        public Vector2 Position { get => _position; set => _position = value; }

        internal ParticleData Data { get => _data; set => _data = value; }

        public Particle(Vector2 pos, ParticleData data)
        {
            _data = data;
            _lifespanLeft = data.lifespan;
            _lifespanAmount = 1f;
            _position = pos;
            _color = data.colorStart;
            _opacity = data.opacityStart;
            _origin = new Vector2(_data.texture.Width / 2, _data.texture.Height / 2);

            if (data.speed != 0)
            {
                // Converti l'angle en radians
                _data.angle = MathHelper.ToRadians(_data.angle);
                // Calcul la direction grace à l'angle
                _direction = new Vector2((float)Math.Sin(_data.angle), -(float)Math.Cos(_data.angle));
            }
            else
            {
                _direction = Vector2.Zero;
            }
        }

        public void Update()
        {
            _lifespanLeft -= Globals.TotalSeconds;
            if (_lifespanLeft <= 0f)
            {
                isFinished = true;
                return;
            }

            // Calcule le temps de vie restant
            _lifespanAmount = _lifespanLeft / _data.lifespan;

            // Melange la couleur finale et la couleur initiale en fonction du lifespan
            _color = Color.Lerp(_data.colorEnd, _data.colorStart, _lifespanAmount);

            // Melange l'opacité finale et l'opacité initiale en fonction du lifepan
            _opacity = MathHelper.Lerp(_data.opacityEnd, _data.opacityStart, _lifespanAmount);

            // Melange la taille finale et la taille initiale en fonction du lifespan, puis ajuste l'échelle par rapport à la largeur de la texture.
            _scale = MathHelper.Lerp(_data.sizeEnd, _data.sizeStart, _lifespanAmount) / _data.texture.Width;

            // Met à jour la position de la particule en fonction de sa direction, de sa vitesse, du temps écoulé et des dimensions de l'écran.
            _position.X += _direction.X * _data.speed * Globals.TotalSeconds / Globals.ScreenWidth;
            _position.Y += _direction.Y * _data.speed * Globals.TotalSeconds / Globals.ScreenHeight;
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(_data.texture, new Vector2(_position.X * Globals.ScreenWidth, _position.Y * Globals.ScreenHeight), null, _color * _opacity, 0f, _origin, _scale, SpriteEffects.None, 1f);
        }
    }
}
