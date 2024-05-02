using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MiFiSy_TPI
{
    internal class Config
    {
        private static XElement _configElement;

        public Config()
        {
            _configElement = XDocument.Load("config.xml").Descendants("Config").FirstOrDefault();
        }

        // Propriétés statiques pour accéder aux valeurs du fichier XML
        public static string AUTHOR_FILE { get => _configElement.Descendants("Author").FirstOrDefault().Value; }
        public static string NAME_SEQUENCE { get => _configElement.Descendants("NameSequence").FirstOrDefault().Value; }
        public static string PATH_MUSIC { get => _configElement.Descendants("PathMusic").FirstOrDefault().Value; }
        public static string PATH_IMG { get => _configElement.Descendants("PathImg").FirstOrDefault().Value; }
        public static string PATH_SAVE_SEQUENCE { get => _configElement.Descendants("PathSaveDequence").FirstOrDefault().Value; }
        public static List<XElement> ALL_MORTAR { get => _configElement.Descendants("Mortar").ToList(); }
        public static Color COLOR_START { get => GetColorFromElement(_configElement.Descendants("ColorStart").FirstOrDefault()); }
        public static Color COLOR_END { get => GetColorFromElement(_configElement.Descendants("ColorEnd").FirstOrDefault()); }
        public static int PARTICLE_RAIN_SIZE { get => Convert.ToInt32(_configElement.Descendants("ParticleRain").FirstOrDefault().Attribute("sizeParticle").Value); }
        public static int PARTICLE_RAIN_NB { get => Convert.ToInt32(_configElement.Descendants("ParticleRain").FirstOrDefault().Attribute("nbParticle").Value); }
        public static float PARTICLE_RAIN_LIFESPAN { get => float.Parse(_configElement.Descendants("ParticleRain").FirstOrDefault().Attribute("lifeSpan").Value); }
        public static int PARTICLE_RAIN_SPEED_DECREASE { get => Convert.ToInt32(_configElement.Descendants("ParticleRain").FirstOrDefault().Attribute("speedDecrease").Value); }
        public static float PARTICLE_RAIN_SPEED { get => float.Parse(_configElement.Descendants("ParticleRain").FirstOrDefault().Attribute("defaultSpeed").Value); }
        public static int COMET_MAIN_SIZE { get => Convert.ToInt32(_configElement.Descendants("Comet").FirstOrDefault().Attribute("sizeMainParticle").Value); }
        public static int COMET_OTHER_SIZE { get => Convert.ToInt32(_configElement.Descendants("Comet").FirstOrDefault().Attribute("sizeOtherParticle").Value); }
        public static float COMET_DEFAULT_SPEED { get => float.Parse(_configElement.Descendants("Comet").FirstOrDefault().Attribute("defaultSpeed").Value); }
        public static float COMET_DEFAULT_LIFESPAN { get => float.Parse(_configElement.Descendants("Comet").FirstOrDefault().Attribute("defaultLifespan").Value); }
        
        /// <summary>
        /// Méthode pour récupérer la couleur à partir d'un élément XML
        /// </summary>
        /// <param name="colorElement">XElement contenant les attributs "r", "g" et "b"</param>
        private static Color GetColorFromElement(XElement colorElement)
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
