using Microsoft.Xna.Framework;
using MiFiSy_TPI.ParticleCreator;
using MiFiSy_TPI.ParticleCreator.Structure;
using SharpDX.X3DAudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MiFiSy_TPI.GameElement.Firework
{
    public class ParticleRain
    {
        private Vector2 _position;
        private List<ParticleEmitter> _lstParticles;
        private float _lifespan;
        private float _timerLife;

        private const int NB_PARTICLE = 360;
        private const int NB_SPEED_DECREASE = 2;

        public ParticleRain(float speed, float lifespan, float distanceFromBorder = 100)
        {
            _lifespan = lifespan;
            _timerLife = 0;
            _position = new Vector2(Globals.RandomFloat(distanceFromBorder, Globals.ScreenWidth - distanceFromBorder) / Globals.ScreenWidth, Globals.RandomFloat(distanceFromBorder, Globals.ScreenHeight / 2) / Globals.ScreenHeight);
            _lstParticles = new List<ParticleEmitter>();

            for (int angle = 1; angle <= NB_PARTICLE; angle++)
            {
                float newSpeed = Globals.RandomFloat(0, speed);
                ParticleEmitterData particleEmitterData = new ParticleEmitterData()
                {
                    interval = 0.01f,
                    emitCount = 1,
                    lifespanMin = lifespan,
                    lifespanMax = lifespan,
                    angle = angle,
                    decreasedLifespan = true,
                    nbDecreasedLifespan = 0.2f,
                    speedMin = newSpeed,
                    speedMax = newSpeed,
                    hasGravity = true,
                    nbGravity = 1,
                    particleData = new ParticleData()
                    {
                        angle = angle,
                        speed = newSpeed,
                        colorStart = Color.OrangeRed,
                        colorEnd = Color.Yellow,
                        sizeStart = 10,
                        sizeEnd = 10,
                    }
                };

                ParticleEmitter p = new ParticleEmitter(_position, particleEmitterData);
                ParticleManager.AddParticleEmitter(p);
                _lstParticles.Add(p);
            }
        }

        public void Update()
        {
            // Diminue la vitesse des particules
            foreach (ParticleEmitter item in _lstParticles)
            {
                ParticleEmitterData newData = item.Data;
                if (newData.speedMax > 0 && newData.speedMin > 0)
                {
                    newData.speedMax -= NB_SPEED_DECREASE;
                    newData.speedMin -= NB_SPEED_DECREASE;
                    item.Data = newData;
                }
            }

            // Supprime en fin de vie
            _timerLife += Globals.TotalSeconds;
            if (_timerLife >= _lifespan)
            {
                foreach (ParticleEmitter item in _lstParticles)
                {
                    ParticleManager.RemoveParticleEmitter(item);
                }
                _lstParticles.Clear();
            }
        }
    }
}
