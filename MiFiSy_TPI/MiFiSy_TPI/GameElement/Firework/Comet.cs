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
    public class Comet
    {
        private Particle _mainParticle;
        private ParticleEmitter _emitter;
        private float _lifespan;
        private float _timerLife;

        public Comet(Vector2 position, float angle, float speed, float lifespan)
        {
            _lifespan = lifespan;
            _timerLife = 0;

            ParticleData particleData = new ParticleData()
            {
                angle = MathHelper.ToDegrees(angle),
                speed = speed,
                lifespan = lifespan,
                colorStart = Color.OrangeRed,
                colorEnd = Color.Yellow,
                sizeStart = 120,
                sizeEnd = 120,
            };
            _mainParticle = new Particle(position, particleData);
            ParticleManager.AddParticle(_mainParticle);
            
            ParticleEmitterData ped = new ParticleEmitterData()
            {
                interval = 0.01f,
                emitCount = 5,
                lifespanMin = lifespan,
                lifespanMax = lifespan,
                angle = MathHelper.ToDegrees(angle),
                randomPosX = true,
                intervalPos = 0.003f,
                decreasedLifespan = true,
                nbDecreasedLifespan = 0.05f,
                speedMin = speed,
                speedMax = speed,
                particleData = new ParticleData()
                {
                    colorStart = Color.OrangeRed,
                    colorEnd = Color.Yellow,
                    sizeStart = 20,
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
            if (_timerLife >= _lifespan)
            {
                ParticleManager.RemoveParticleEmitter(_emitter);
                ParticleManager.RemoveParticle(_mainParticle);
            }
        }
    }
}
