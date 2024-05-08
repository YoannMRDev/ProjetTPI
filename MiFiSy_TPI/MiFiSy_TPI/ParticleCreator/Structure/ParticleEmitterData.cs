/*
 * Auteur : Yoann Meier
 * Date : 06/05/2024
 * Projet : Projet TPI, application de simulation de feux d'artifices en 2D
 * Description de la page : Struct contenant les variables d'un emitteur de particules (vient de : https://www.youtube.com/watch?v=-4_kj_gyWRY)
 */
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
        public ParticleEmitterData()
        {
        }
    }
}
