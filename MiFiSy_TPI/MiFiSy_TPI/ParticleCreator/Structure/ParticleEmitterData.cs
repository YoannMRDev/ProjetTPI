using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiFiSy_TPI.ParticleCreator.Structure
{
    internal struct ParticleEmitterData
    {
        public ParticleData particleData = new ParticleData();
        public float angle = 0f;
        public float angleVariance = 0f;
        public float lifespanMin = 0.1f;
        public float lifespanMax = 2f;
        public float speedMin = 10f;
        public float speedMax = 100f;
        public float interval = 1f;
        public int emitCount = 1;
        public bool decreasedLifespan = false;
        public float nbDecreasedLifespan = 0.05f;
        public bool randomPosX = false;
        public float intervalPos = 0.01f;
        public bool hasGravity = false;
        public float nbGravity = 1f;
        public ParticleEmitterData()
        {
        }
    }
}
