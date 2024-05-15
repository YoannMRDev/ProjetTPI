using System;
using System.Collections.Generic;
/*
 * Auteur : Yoann Meier
 * Date : 15/05/2024
 * Projet : Projet TPI, application de simulation de feux d'artifices en 2D
 * Description de la page : Class d'une particule (Viens de : https://www.youtube.com/watch?v=-4_kj_gyWRY)
 */
namespace MiFiSy_TPI.ParticleCreator
{
    internal class ParticleManager
    {
        private static List<Particle> _particles = new List<Particle>();
        private static List<ParticleEmitter> _particleEmitters = new List<ParticleEmitter>();

        /// <summary>
        /// Ajoute une particule dans la liste
        /// </summary>
        public static void AddParticle(Particle p)
        {
            _particles.Add(p);
        }

        /// <summary>
        /// Ajoute un émetteur de particules
        /// </summary>
        public static void AddParticleEmitter(ParticleEmitter e)
        {
            _particleEmitters.Add(e);
        }

        public static void Update()
        {
            // Supprime les particules et émetteur finis
            _particles.RemoveAll(p => p.isFinished);
            _particleEmitters.RemoveAll(p => p.destroy);

            try
            {
                _particles.ForEach(p => p.Update());
                _particleEmitters.ForEach(e => e.Update());
            }
            catch (InvalidOperationException) { /* Il arrive parfois qu'une particule ou émetteur soit ajouté pendant la mise à jour */ }
        }

        /// <summary>
        /// Supprime une paricule
        /// </summary>
        public static void RemoveParticle(Particle p)
        {
            _particles.Remove(p);
        }

        /// <summary>
        /// Supprime un émetteur de paricules
        /// </summary>
        public static void RemoveParticleEmitter(ParticleEmitter p)
        {
            _particleEmitters.Remove(p);
        }

        /// <summary>
        /// Affiche les particules
        /// </summary>
        public static void Draw()
        {
            try
            {
                _particles.ForEach(p => p.Draw());
            }
            catch (InvalidOperationException) { /* Il arrive parfois qu'une particule ou émetteur soit ajouté pendant l'affichage */ }
        }

        /// <summary>
        /// Supprime toutes les particules et émetteur
        /// </summary>
        public static void ClearParticle()
        {
            _particleEmitters.Clear();
            _particles.Clear();
        }
    }
}
