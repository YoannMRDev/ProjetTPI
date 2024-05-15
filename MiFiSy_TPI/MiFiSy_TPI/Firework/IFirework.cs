using Microsoft.Xna.Framework;
/*
 * Auteur : Yoann Meier
 * Date : 15/05/2024
 * Projet : Projet TPI, application de simulation de feux d'artifices en 2D
 * Description de la page : Interface des particules pour n'avoir qu'une seule liste
 */
namespace MiFiSy_TPI.Firework
{
    public interface IFirework
    {
        /// <summary>
        /// position du feu d'artifice au départ
        /// </summary>
        Vector2 StartPosition { get; set; }

        /// <summary>
        /// durée de vie du feu d'artifice
        /// </summary>
        float Lifespan { get; set; }

        /// <summary>
        /// Temps après le début du mode libre où ce feu d'artifice est créé
        /// </summary>
        float LaunchTime { get; set; }

        /// <summary>
        /// Vitesse de départ
        /// </summary>
        float StartSpeed { get; set; }

        /// <summary>
        /// Méthode update pour supprimer les anciennes particules
        /// </summary>
        void Update();
    }
}
