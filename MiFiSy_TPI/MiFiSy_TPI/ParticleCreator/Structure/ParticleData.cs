﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiFiSy_TPI.ParticleCreator.Structure
{
    internal struct ParticleData
    {
        public Texture2D texture = Globals.Content.Load<Texture2D>("particle");
        public float lifespan = 2f;
        public Color colorStart = Color.Yellow;
        public Color colorEnd = Color.Red;
        public float opacityStart = 1f;
        public float opacityEnd = 0f;
        public float sizeStart = 32f;
        public float sizeEnd = 4f;
        public float speed = 100f;
        public float angle = 0f;

        public ParticleData()
        {
        }
    }
}