using Microsoft.Xna.Framework;
using MiFiSy_TPI.ParticleCreator;
using MiFiSy_TPI.ParticleCreator.Structure;
using System;
using System.Collections.Generic;
/*
 * Auteur : Yoann Meier
 * Date : 06/05/2024
 * Projet : Projet TPI, application de simulation de feux d'artifices en 2D
 * Description de la page : Class d'une pluie de particule, version 2
 */
namespace MiFiSy_TPI.Firework
{
    public class ParticleRain : IFirework
    {
        private List<Particle> _lstMainParticles;
        private float _lifespan;
        private float _timerLife;
        private float _timerSpawn;
        private float _launchTime;
        private Vector2 _startPosition;
        private float _startSpeed;
        private float _nbParticle;
        private Color _colorStart;
        private Color _colorEnd;
        private float _size;

        public float Lifespan { get => _lifespan; set => _lifespan = value; }
        public float LaunchTime { get => _launchTime; set => _launchTime = value; }
        public Vector2 StartPosition { get => _startPosition; set => _startPosition = value; }
        public float StartSpeed { get => _startSpeed; set => _startSpeed = value; }

        /// <summary>
        /// Constructeur de la classe utilisée dans le jeu libre : créer la pluie de particule en fonction de paramètre du fichier de configuration
        /// </summary>
        /// <param name="speed">vitesse de départ du feu d'artifice</param>
        /// <param name="lifespan">durée de vie du feu d'artifice</param>
        /// <param name="launchTime">Le temps à laquelle l'effet a été crée, seulement utilisé pour la sauvegarde</param>
        /// <param name="distanceFromBorder">distance pour ne pas créer la particule en dehors ou sur le bord de l'écran</param>
        //[:constructeur:]
        public ParticleRain(float speed, float lifespan, float launchTime, float distanceFromBorder = 100)
        {
            LaunchTime = launchTime;
            Lifespan = lifespan;
            StartSpeed = speed;
            _nbParticle = Config.PARTICLE_RAIN_NB;
            _colorStart = Config.COLOR_PARTICLE_RAIN_START;
            _colorEnd = Config.COLOR_PARTICLE_RAIN_END;
            _size = Config.PARTICLE_RAIN_SIZE;

            _timerLife = 0;
            _timerSpawn = 0;
            // Position aléatoire du feu d'artifice sur la partie haute de l'écran
            StartPosition = new Vector2(Globals.RandomFloat(distanceFromBorder, Globals.ScreenWidth - distanceFromBorder) / Globals.ScreenWidth, Globals.RandomFloat(distanceFromBorder, Globals.ScreenHeight / 2) / Globals.ScreenHeight);
            _lstMainParticles = new List<Particle>();

            for (int i = 0; i < _nbParticle; i++)
            {
                float angle = 360 / _nbParticle * i;
                // Vitesse aléatoire entre 0 et le maximum
                float newSpeed = Globals.RandomFloat(0, speed);
                ParticleData particleData = new ParticleData()
                {
                    angle = angle,
                    speed = newSpeed,
                    colorStart = _colorStart,
                    colorEnd = _colorEnd,
                    sizeStart = _size,
                    sizeEnd = _size,
                    lifespan = Lifespan,
                };
                Particle p = new Particle(StartPosition, particleData);
                _lstMainParticles.Add(p);
                ParticleManager.AddParticle(p);
            }
        }
        //[:finconstructeur:]
        /// <summary>
        /// Constructeur de la classe utilisée dans le jeu replay : créer la pluie de particules en fonction de paramètre du fichier qui est rejoué
        /// </summary>
        /// <param name="position">position de départ</param>
        /// <param name="speed">vitesse de départ du feu d'artifice</param>
        /// <param name="lifespan">durée de vie du feu d'artifice</param>
        /// <param name="colorStart">couleur de départ</param>
        /// <param name="colorEnd">couleur de fin</param>
        /// <param name="size">taille des particules</param>
        /// <param name="nbParticle">nombre de particule a générer</param>
        public ParticleRain(Vector2 position, float speed, float lifespan, Color colorStart, Color colorEnd, float size, float nbParticle)
        {
            LaunchTime = 0f;
            Lifespan = lifespan;
            StartSpeed = speed;
            _nbParticle = nbParticle;
            _colorStart = colorStart;
            _colorEnd = colorEnd;
            _size = size;

            _timerLife = 0;
            _timerSpawn = 0;
            StartPosition = position;

            _lstMainParticles = new List<Particle>();

            for (int i = 0; i < _nbParticle; i++)
            {
                float angle = 360 / _nbParticle * i;
                // Vitesse aléatoire entre 0 et le maximum
                float newSpeed = Globals.RandomFloat(0, speed);
                ParticleData particleData = new ParticleData()
                {
                    angle = angle,
                    speed = newSpeed,
                    colorStart = _colorStart,
                    colorEnd = _colorEnd,
                    sizeStart = _size,
                    sizeEnd = _size,
                    lifespan = lifespan,
                };
                Particle p = new Particle(StartPosition, particleData);
                _lstMainParticles.Add(p);
                ParticleManager.AddParticle(p);
            }
        }

        //[:updatedebut:]
        public void Update()
        {
            _timerLife += Globals.TotalSeconds;
            _timerSpawn += Globals.TotalSeconds;
            // Supprime en fin de vie
            if (_timerLife >= Lifespan)
            {
                _lstMainParticles.Clear();
            }

            if (_lstMainParticles.Count != 0)
            {
                if (_timerSpawn >= Config.PARTICLE_RAIN_TIME_SPAWN)
                {
                    // Ajoute une particule immobile sur chaque particule en mouvement
                    for (int i = 0; i < _nbParticle; i++)
                    {
                        ParticleData particleData = new ParticleData()
                        {
                            angle = MathHelper.ToDegrees(_lstMainParticles[i].Data.angle),
                            speed = 0,
                            colorStart = _colorStart,
                            colorEnd = _colorEnd,
                            sizeStart = _size,
                            sizeEnd = _size,
                            lifespan = Lifespan - _timerLife,
                        };
                        Particle p = new Particle(_lstMainParticles[i].Position, particleData);
                        ParticleManager.AddParticle(p);
                    }
                    _timerSpawn = 0;
                }
                
                // Si un tiers du temps total est passé, les particules en movement tombent
                if (_timerLife >= Lifespan / 3)
                {
                    foreach (Particle item in _lstMainParticles)
                    {
                        ParticleData data = item.Data;
                        int angleAdd = MathHelper.ToDegrees(data.angle) < 180 ? 1 : -1;
                        data.angle = MathHelper.ToDegrees(data.angle) + angleAdd;
                        item.Data = data;
                        item.SetAngleAndDirection();
                    }
                }
            }
        }
        //[:updatefin:]
    }
}
