using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MiFiSy_TPI.ParticleCreator.Structure;

namespace MiFiSy_TPI.ParticleCreator
{
    internal class ParticleManager
    {
        private static readonly List<Particle> _particles = new List<Particle>();
        private static readonly List<ParticleEmitter> _particleEmitters = new List<ParticleEmitter>();

        public ParticleManager()
        {
            /*
            ParticleEmitterData ped = new ParticleEmitterData()
            {
                interval = 0.01f,
                emitCount = 1000,
                angleVariance = 0,
                lifespanMin = 0.8f,
                lifespanMax = 1,
                speedMin = 50,
                speedMax = 100,
                randomPosX = true,
                intervalPos = 0.5f,
                particleData = new ParticleData()
                {
                    colorStart = Color.OrangeRed,
                    colorEnd = Color.Yellow,
                    sizeStart = 8,
                    sizeEnd = 32,
                }
            };
            AddParticleEmitter(new ParticleEmitter(new Vector2(0.5f, 1f), ped));
            */
            
            /*ParticleEmitterData ped3 = new ParticleEmitterData()
            {
                interval = 0.01f,
                angleVariance = 180,
                emitCount = 20,
                particleData = new ParticleData()
                {
                    colorStart = Color.OrangeRed,
                    colorEnd = Color.Yellow,
                }
            };

            AddParticleEmitter(new ParticleEmitter(new Vector2(0.2f, 0.4f), ped3));
            */
        }

        public static void CreateComete(Vector2 emitposition, float angle, float speed, float lifespan)
        {
            ParticleEmitterData ped2 = new ParticleEmitterData()
            {
                interval = 0,
                emitCount = 0,
                lifespanMin = lifespan,
                lifespanMax = lifespan,
                angle = MathHelper.ToDegrees(angle),
                speedMin = speed,
                speedMax = speed,
                particleData = new ParticleData()
                {
                    colorStart = Color.OrangeRed,
                    colorEnd = Color.Yellow,
                    sizeStart = 120,
                    sizeEnd = 120,
                }
            };

            ParticleEmitter particleEmitter = new ParticleEmitter(emitposition, ped2);
            particleEmitter.Emit(emitposition);
            AddParticleEmitter(particleEmitter);

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
            AddParticleEmitter(new ParticleEmitter(emitposition, ped));
        }

        public static void AddParticle(Particle p)
        {
            _particles.Add(p);
        }

        public static void AddParticleEmitter(ParticleEmitter e)
        {
            _particleEmitters.Add(e);
        }

        public static void Update()
        {
            _particles.RemoveAll(p => p.isFinished);
            _particleEmitters.RemoveAll(p => p.destroy);
            _particles.ForEach(p => p.Update());
            _particleEmitters.ForEach(e => e.Update());

            Debug.Print(_particles.Count.ToString());
        }

        public static void Draw()
        {
            _particles.ForEach(p => p.Draw());
        }
    }
}
