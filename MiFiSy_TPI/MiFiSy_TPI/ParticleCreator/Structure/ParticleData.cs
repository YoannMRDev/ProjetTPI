using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
/*
 * Auteur : Yoann Meier
 * Date : 06/05/2024
 * Projet : Projet TPI, application de simulation de feux d'artifices en 2D
 * Description de la page : Struct contenant les variables d'une particule (vient de : https://www.youtube.com/watch?v=-4_kj_gyWRY)
 */
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