using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MiFiSy_TPI.ParticleCreator;
using MiFiSy_TPI.ParticleCreator.Structure;
using SharpDX.X3DAudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiFiSy_TPI.GameElement.Firework
{
    public class Comet : IFirework
    {
        private Particle _mainParticle;
        private ParticleEmitter _emitter;
        private float _lifespan;
        private float _timerLife;

        private float _launchTime;
        private Vector2 _startPosition;
        private float _startAngle;
        private float _startSpeed;

        public float LaunchTime { get => _launchTime; set => _launchTime = value; }
        public Vector2 StartPosition { get => _startPosition; set => _startPosition = value; }
        internal Particle MainParticle { get => _mainParticle; set => _mainParticle = value; }
        public float StartAngle { get => _startAngle; set => _startAngle = value; }
        public float StartSpeed { get => _startSpeed; set => _startSpeed = value; }
        public float Lifespan { get => _lifespan; set => _lifespan = value; }

        public Comet(Vector2 position, float angle, float speed, float lifespan, float launchTime)
        {
            LaunchTime = launchTime;
            StartPosition = position;
            StartAngle = MathHelper.ToDegrees(angle);
            StartSpeed = speed;
            Lifespan = lifespan;
            _timerLife = 0;

            ParticleData particleData = new ParticleData()
            {
                angle = StartAngle,
                speed = StartSpeed,
                lifespan = Lifespan,
                colorStart = Config.COLOR_START,
                colorEnd = Config.COLOR_END,
                sizeStart = Config.COMET_MAIN_SIZE,
                sizeEnd = Config.COMET_MAIN_SIZE,
            };
            _mainParticle = new Particle(position, particleData);
            ParticleManager.AddParticle(_mainParticle);
            
            ParticleEmitterData ped = new ParticleEmitterData()
            {
                interval = 0.01f,
                emitCount = 5,
                lifespanMin = Lifespan,
                lifespanMax = Lifespan,
                angle = StartAngle,
                randomPosX = true,
                intervalPos = 0.003f,
                decreasedLifespan = true,
                nbDecreasedLifespan = 0.05f,
                speedMin = StartSpeed,
                speedMax = StartSpeed,
                particleData = new ParticleData()
                {
                    colorStart = Config.COLOR_START,
                    colorEnd = Config.COLOR_END,
                    sizeStart = Config.COMET_OTHER_SIZE,
                    sizeEnd = Config.COMET_OTHER_SIZE,
                }
            };
            _emitter = new ParticleEmitter(_mainParticle.Position, ped);
            ParticleManager.AddParticleEmitter(_emitter);
        }

        public void Update()
        {
            if (_emitter.Data.particleData.speed != _mainParticle.Data.speed || _emitter.Data.particleData.angle != _mainParticle.Data.angle)
            {
                // Met à jour les données entre tête et queue
                ParticleEmitterData newData = _emitter.Data;
                newData.particleData.angle = _mainParticle.Data.angle;
                newData.particleData.speed = _mainParticle.Data.speed;
                _emitter.Data = newData;
            }

            // Supprime en fin de vie
            _timerLife += Globals.TotalSeconds;
            if (_timerLife >= Lifespan)
            {
                ParticleManager.RemoveParticleEmitter(_emitter);
                ParticleManager.RemoveParticle(_mainParticle);
            }
        }
    }
}
