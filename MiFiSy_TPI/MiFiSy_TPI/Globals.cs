using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MiFiSy_TPI.GameElement.Firework;
using System;
using System.Collections.Generic;

namespace MiFiSy_TPI
{
    public static class Globals
    {
        public static float TotalSeconds { get; set; }

        public enum AllPage
        {
            Accueil,
            Jeu,
        }

        public static AllPage ActualPage {  get; set; }

        public static ContentManager Content { get; set; }

        public static SpriteBatch SpriteBatch { get; set; }

        public static SpriteFont FontButton { get; set; }

        public static GraphicsDevice GraphicsDevice { get; set; }

        public static Random Random { get; set; } = new Random();

        public static int ScreenWidth { get; set; }

        public static int ScreenHeight { get; set; }

        public static void Update(GameTime gt)
        {
            TotalSeconds = (float)gt.ElapsedGameTime.TotalSeconds;
        }

        public static float RandomFloat(float min, float max)
        {
            return (float)(Random.NextDouble() * (max - min)) + min;
        }

        public static int RandomInt(int min, int max)
        {
            return Random.Next(min, max + 1);
        }

        public static List<Comete> LstComete {  get; set; }

    }
}