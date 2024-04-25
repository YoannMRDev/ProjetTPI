using Microsoft.Xna.Framework;
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
    internal class Comete
    {
        private Particle mainParticle;
        private ParticleEmitter emitter;

        public Comete(Vector2 position, float angle, float speed, float lifespan)
        {
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
            mainParticle = new Particle(position, particleData);

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

            emitter = new ParticleEmitter(mainParticle.Position, ped);
            ParticleManager.AddParticleEmitter(emitter);
        }
    }
}
