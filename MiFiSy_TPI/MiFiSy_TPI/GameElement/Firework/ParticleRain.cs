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
    public class ParticleRain : IFirework
    {
        private List<ParticleEmitter> _lstParticlesEmitter;
        private float _lifespan;
        private float _timerLife;
        
        private float _launchTime;
        private Vector2 _startPosition;
        private float _startSpeed;

        public float Lifespan { get => _lifespan; set => _lifespan = value; }
        public float LaunchTime { get => _launchTime; set => _launchTime = value; }
        public Vector2 StartPosition { get => _startPosition; set => _startPosition = value; }
        public float StartSpeed { get => _startSpeed; set => _startSpeed = value; }

        public ParticleRain(float speed, float lifespan, float launchTime ,float distanceFromBorder = 100)
        {
            LaunchTime = launchTime;
            Lifespan = lifespan;
            StartSpeed = speed;
            
            _timerLife = 0;
            StartPosition = new Vector2(Globals.RandomFloat(distanceFromBorder, Globals.ScreenWidth - distanceFromBorder) / Globals.ScreenWidth, Globals.RandomFloat(distanceFromBorder, Globals.ScreenHeight / 2) / Globals.ScreenHeight);
            _lstParticlesEmitter = new List<ParticleEmitter>();

            for (int i = 1; i <= Config.PARTICLE_RAIN_NB; i++)
            {
                float angle = 360 / Config.PARTICLE_RAIN_NB * i;
                float newSpeed = Globals.RandomFloat(0, speed);
                ParticleEmitterData particleEmitterData = new ParticleEmitterData()
                {
                    interval = 0.01f,
                    emitCount = 1,
                    lifespanMin = Lifespan,
                    lifespanMax = Lifespan,
                    angle = angle,
                    decreasedLifespan = true,
                    nbDecreasedLifespan = 0.2f,
                    speedMin = newSpeed,
                    speedMax = newSpeed,
                    hasGravity = true,
                    particleData = new ParticleData()
                    {
                        angle = angle,
                        speed = newSpeed,
                        colorStart = Config.COLOR_START,
                        colorEnd = Config.COLOR_END,
                        sizeStart = Config.PARTICLE_RAIN_SIZE,
                        sizeEnd = Config.PARTICLE_RAIN_SIZE,
                    }
                };

                ParticleEmitter p = new ParticleEmitter(StartPosition, particleEmitterData);
                ParticleManager.AddParticleEmitter(p);
                _lstParticlesEmitter.Add(p);
            }
        }

        public ParticleRain(Vector2 position, float speed, float lifespan, Color colorStart, Color colorEnd, float size, float nbParticle)
        {
            LaunchTime = 0f;
            Lifespan = lifespan;
            StartSpeed = speed;

            _timerLife = 0;
            StartPosition = position;
            _lstParticlesEmitter = new List<ParticleEmitter>();

            for (int i = 1; i <= nbParticle; i++)
            {
                float angle = 360 / nbParticle * i;
                float newSpeed = Globals.RandomFloat(0, speed);
                ParticleEmitterData particleEmitterData = new ParticleEmitterData()
                {
                    interval = 0.01f,
                    emitCount = 1,
                    lifespanMin = Lifespan,
                    lifespanMax = Lifespan,
                    angle = angle,
                    decreasedLifespan = true,
                    nbDecreasedLifespan = 0.2f,
                    speedMin = newSpeed,
                    speedMax = newSpeed,
                    hasGravity = true,
                    particleData = new ParticleData()
                    {
                        angle = angle,
                        speed = newSpeed,
                        colorStart = colorStart,
                        colorEnd = colorEnd,
                        sizeStart = size,
                        sizeEnd = size,
                    }
                };

                ParticleEmitter p = new ParticleEmitter(StartPosition, particleEmitterData);
                ParticleManager.AddParticleEmitter(p);
                _lstParticlesEmitter.Add(p);
            }
        }


        public void Update()
        {
            // Diminue la vitesse des particules
            foreach (ParticleEmitter item in _lstParticlesEmitter)
            {
                ParticleEmitterData newData = item.Data;
                if (newData.speedMax > 0 && newData.speedMin > 0)
                {
                    newData.speedMax -= Config.PARTICLE_RAIN_SPEED_DECREASE;
                    newData.speedMin -= Config.PARTICLE_RAIN_SPEED_DECREASE;
                    item.Data = newData;
                }
            }

            // Supprime en fin de vie
            _timerLife += Globals.TotalSeconds;
            if (_timerLife >= Lifespan)
            {
                foreach (ParticleEmitter item in _lstParticlesEmitter)
                {
                    ParticleManager.RemoveParticleEmitter(item);
                }
                _lstParticlesEmitter.Clear();
            }
        }
    }
}
