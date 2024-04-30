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
        private static List<Particle> _particles = new List<Particle>();
        private static List<ParticleEmitter> _particleEmitters = new List<ParticleEmitter>();

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

            try
            {
                _particles.ForEach(p => p.Update());
                _particleEmitters.ForEach(e => e.Update());
            }
            catch (InvalidOperationException)
            {
                Debug.Print("Ajout de particules pendant l'update");
            }
        }

        public static void RemoveParticle(Particle p)
        {
            _particles.Remove(p);
        }

        public static void RemoveParticleEmitter(ParticleEmitter p)
        {
            _particleEmitters.Remove(p);
        }

        public static void Draw()
        {
            try
            {
                _particles.ForEach(p => p.Draw());
            }
            catch (InvalidOperationException)
            {
                Debug.Print("Ajout de particules pendant le draw");
            }
        }
    }
}
