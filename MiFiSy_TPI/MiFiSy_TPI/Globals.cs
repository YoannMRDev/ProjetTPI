using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MiFiSy_TPI.GameElement;
using MiFiSy_TPI.GameElement.Firework;
using System;
using System.Collections.Generic;

namespace MiFiSy_TPI
{
    internal static class Globals
    {
        public static float TotalSeconds { get; set; }

        public enum AllPage
        {
            Home,
            Game,
        }

        public static AllPage ActualPage {  get; set; }

        public static ContentManager Content { get; set; }

        public static SpriteBatch SpriteBatch { get; set; }

        public static SpriteFont FontButton { get; set; }

        public static GraphicsDevice GraphicsDevice { get; set; }

        public static Random Random { get; set; } = new Random();

        public static int ScreenWidth { get; set; }

        public static int ScreenHeight { get; set; }

        public static string MusicSelectedName { get; set; }

        public static string ReplaySelectedName { get; set; }

        public static Home home { get; set; }

        public static GameManager GameManager { get; set; }


        public static void Update(GameTime gt)
        {
            TotalSeconds = (float)gt.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Retourne un float aléatoire entre min et max
        /// </summary>
        /// <param name="min">nombre minimum</param>
        /// <param name="max">nombre maximum</param>
        /// <returns>nombre aléatoire</returns>
        public static float RandomFloat(float min, float max)
        {
            return (float)(Random.NextDouble() * (max - min)) + min;
        }

        /// <summary>
        /// Retourne un nombre aléatoire entre min et max
        /// </summary>
        /// <param name="min">nombre minimum</param>
        /// <param name="max">nombre maximum</param>
        /// <returns>nombre aléatoire</returns>
        public static int RandomInt(int min, int max)
        {
            return Random.Next(min, max + 1);
        }

        /// <summary>
        /// Liste de feu d'artifice de la séquence (mode libre)
        /// </summary>
        public static List<IFirework> LstFirework {  get; set; }
    }
}