using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MiFiSy_TPI.Firework;
using MiFiSy_TPI.Manager;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
/*
 * Auteur : Yoann Meier
 * Date : 06/05/2024
 * Projet : Projet TPI, application de simulation de feux d'artifices en 2D
 * Description de la page : Page contenant des valeurs static nécéssaires dans plusieurs pages
 */
namespace MiFiSy_TPI
{
    internal static class Globals
    {
        public static float TotalSeconds { get; set; }

        /// <summary>
        /// Enum de toutes les pages de l'application
        /// </summary>
        public enum AllPage
        {
            Home,
            Game,
        }

        /// <summary>
        /// Page actuel
        /// </summary>
        public static AllPage ActualPage {  get; set; }

        public static ContentManager Content { get; set; }

        public static SpriteBatch SpriteBatch { get; set; }

        public static SpriteFont FontButton { get; set; }

        public static GraphicsDevice GraphicsDevice { get; set; }

        public static Random Random { get; set; } = new Random();

        /// <summary>
        /// Largeur de l'écran
        /// </summary>
        public static int ScreenWidth { get; set; }

        /// <summary>
        /// Hauteur de l'écran
        /// </summary>
        public static int ScreenHeight { get; set; }

        public static string MusicSelectedName { get; set; }

        public static Home home { get; set; }

        public static GameManager GameManager { get; set; }

        /// <summary>
        /// Liste de feu d'artifice
        /// </summary>
        public static List<IFirework> LstFirework { get; set; }

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
        /// Méthode pour récupérer la couleur à partir d'un élément XML
        /// </summary>
        /// <param name="colorElement">XElement contenant les attributs "r", "g" et "b"</param>
        public static Color GetColorFromElement(XElement colorElement)
        {
            if (colorElement.Attribute("r") != null && colorElement.Attribute("g") != null && colorElement.Attribute("b") != null)
            {
                int r = Convert.ToInt32(colorElement.Attribute("r").Value);
                int g = Convert.ToInt32(colorElement.Attribute("g").Value);
                int b = Convert.ToInt32(colorElement.Attribute("b").Value);
                return new Color(r, g, b);
            }
            return Color.White;
        }
    }
}