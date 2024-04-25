using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiFiSy_TPI.ParticleCreator.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiFiSy_TPI.ParticleCreator
{
    internal class ParticleEmitter
    {
        private ParticleEmitterData data;
        private float _intervalLeft;
        private Vector2 _emitPosition;
        public bool destroy;

        public ParticleEmitter(Vector2 emitPosition, ParticleEmitterData data)
        {
            _emitPosition = emitPosition;
            this.data = data;
            _intervalLeft = data.interval;
            destroy = false;
        }

        /// <summary>
        /// Émet une nouvelle particule avec une position
        /// </summary>
        /// <param name="pos">La position à partir de laquelle émettre la particule</param>
        public void Emit(Vector2 pos)
        {
            ParticleData d = data.particleData;
            // Random lifespan, speed, angle
            d.lifespan = Globals.RandomFloat(data.lifespanMin, data.lifespanMax);
            d.speed = Globals.RandomFloat(data.speedMin, data.speedMax);
            d.angle = Globals.RandomFloat(data.angle - data.angleVariance, data.angle + data.angleVariance);

            if (data.randomPosX)
            {
                // Position random X
                float xPosition = pos.X * Globals.ScreenWidth;
                float randomX = Globals.RandomFloat(xPosition - data.intervalPos * Globals.ScreenWidth, xPosition + data.intervalPos * Globals.ScreenWidth) / Globals.ScreenWidth;
                pos.X = randomX;
            }
            Particle p = new Particle(pos, d);
            ParticleManager.AddParticle(p);
        }

        public void Update()
        {
            _intervalLeft -= Globals.TotalSeconds;
            if (_intervalLeft <= 0f)
            {
                // Réinitialise le temps restant
                _intervalLeft += data.interval;
                // Emet les nouvelles particules
                for (int i = 0; i < data.emitCount; i++)
                {
                    Emit(_emitPosition);
                }
                if (data.decreasedLifespan)
                {
                    data.lifespanMin -= data.nbDecreasedLifespan;
                    data.lifespanMax -= data.nbDecreasedLifespan;
                }

            }

        }
    }
}
