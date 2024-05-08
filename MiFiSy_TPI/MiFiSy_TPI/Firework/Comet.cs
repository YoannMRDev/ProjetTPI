using Microsoft.Xna.Framework;
using MiFiSy_TPI.ParticleCreator;
using MiFiSy_TPI.ParticleCreator.Structure;
/*
 * Auteur : Yoann Meier
 * Date : 06/05/2024
 * Projet : Projet TPI, application de simulation de feux d'artifices en 2D
 * Description de la page : Classe de création du feu d'artifice de la comète
 */
namespace MiFiSy_TPI.Firework
{
    public class Comet : IFirework
    {
        private Particle _mainParticle;
        private ParticleEmitter _emitter;
        private float _lifespan;
        private float _timerLife;
        private float _launchTime;
        private Vector2 _startPosition;
        private float _startAngle;
        private float _startSpeed;

        public float LaunchTime { get => _launchTime; set => _launchTime = value; }
        public Vector2 StartPosition { get => _startPosition; set => _startPosition = value; }
        internal Particle MainParticle { get => _mainParticle; set => _mainParticle = value; }
        public float StartAngle { get => _startAngle; set => _startAngle = value; }
        public float StartSpeed { get => _startSpeed; set => _startSpeed = value; }
        public float Lifespan { get => _lifespan; set => _lifespan = value; }


        /// <summary>
        /// Constructeur de la classe utilisée dans le jeu libre : créer la comète en fonction de paramètre du fichier de configuration
        /// </summary>
        /// <param name="position">Position de départ de la comète</param>
        /// <param name="angle">angle de la comète</param>
        /// <param name="speed">vitesse de la comète</param>
        /// <param name="lifespan">durée de vie de la comète</param>
        /// <param name="launchTime">Le temps à laquelle l'effet a été crée, seulement utilisé pour la sauvegarde</param>
        public Comet(Vector2 position, float angle, float speed, float lifespan, float launchTime)
        {
            LaunchTime = launchTime;
            StartPosition = position;
            StartAngle = MathHelper.ToDegrees(angle);
            StartSpeed = speed;
            Lifespan = lifespan;
            _timerLife = 0;

            // Créer la particule principale, la tête
            ParticleData particleData = new ParticleData()
            {
                angle = StartAngle,
                speed = StartSpeed,
                lifespan = Lifespan,
                colorStart = Config.COLOR_START_COMET,
                colorEnd = Config.COLOR_END_COMET,
                sizeStart = Config.COMET_MAIN_SIZE,
                sizeEnd = Config.COMET_MAIN_SIZE,
            };
            _mainParticle = new Particle(position, particleData);
            ParticleManager.AddParticle(_mainParticle);

            // créer l'émetteur qui suit la tête, la queue
            ParticleEmitterData ped = new ParticleEmitterData()
            {
                interval = 0.01f,
                emitCount = 5,
                lifespanMin = Lifespan,
                lifespanMax = Lifespan,
                angle = StartAngle,
                randomPosX = true,
                intervalPos = 0.003f,
                decreasedLifespan = true,
                nbDecreasedLifespan = 0.05f,
                speedMin = StartSpeed,
                speedMax = StartSpeed,
                particleData = new ParticleData()
                {
                    colorStart = Config.COLOR_START_COMET,
                    colorEnd = Config.COLOR_END_COMET,
                    sizeStart = Config.COMET_OTHER_SIZE,
                    sizeEnd = Config.COMET_OTHER_SIZE,
                }
            };
            _emitter = new ParticleEmitter(_mainParticle.Position, ped);
            ParticleManager.AddParticleEmitter(_emitter);
        }

        /// <summary>
        /// Constructeur de la classe utilisée dans le jeu replay : créer la comète en fonction de paramètre du fichier qui est rejoué
        /// </summary>
        /// <param name="position">Position de départ de la comète</param>
        /// <param name="angle">angle de la comète</param>
        /// <param name="speed">vitesse de la comète</param>
        /// <param name="lifespan">durée de vie de la comète</param>
        /// <param name="colorStart">couleur de départ de la comète</param>
        /// <param name="colorEnd">couleur de fin de la comète</param>
        /// <param name="mainSize">taille des particules de la tête</param>
        /// <param name="otherSize">taille des particules de la queue</param>
        public Comet(Vector2 position, float angle, float speed, float lifespan, Color colorStart, Color colorEnd, float mainSize, float otherSize)
        {
            LaunchTime = 0f;
            StartPosition = position;
            StartAngle = angle;
            StartSpeed = speed;
            Lifespan = lifespan;
            _timerLife = 0;

            // Créer la particule principale, la tête
            ParticleData particleData = new ParticleData()
            {
                angle = StartAngle,
                speed = StartSpeed,
                lifespan = Lifespan,
                colorStart = colorStart,
                colorEnd = colorEnd,
                sizeStart = mainSize,
                sizeEnd = mainSize,
            };
            _mainParticle = new Particle(position, particleData);
            ParticleManager.AddParticle(_mainParticle);

            // créer l'émetteur qui suit la tête, la queue
            ParticleEmitterData ped = new ParticleEmitterData()
            {
                interval = 0.01f,
                emitCount = 5,
                lifespanMin = Lifespan,
                lifespanMax = Lifespan,
                angle = StartAngle,
                randomPosX = true,
                intervalPos = 0.003f,
                decreasedLifespan = true,
                nbDecreasedLifespan = 0.05f,
                speedMin = StartSpeed,
                speedMax = StartSpeed,
                particleData = new ParticleData()
                {
                    colorStart = colorStart,
                    colorEnd = colorEnd,
                    sizeStart = otherSize,
                    sizeEnd = otherSize,
                }
            };
            _emitter = new ParticleEmitter(_mainParticle.Position, ped);
            ParticleManager.AddParticleEmitter(_emitter);
        }

        public void Update()
        {
            _timerLife += Globals.TotalSeconds;
            if (_timerLife >= Lifespan)
            {
                ParticleManager.RemoveParticleEmitter(_emitter);
                ParticleManager.RemoveParticle(_mainParticle);
            }
        }
    }
}
