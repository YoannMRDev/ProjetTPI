using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
/*
 * Auteur : Yoann Meier
 * Date : 06/05/2024
 * Projet : Projet TPI, application de simulation de feux d'artifices en 2D
 * Description de la page : Classe pour gérer la souris
 */
namespace MiFiSy_TPI.Manager
{
    internal class InputManager
    {
        private static MouseState _lastMouseState;
        public static bool HasClicked { get; private set; }
        public static Vector2 MousePosition { get; private set; }

        public static void Update()
        {
            var mouseState = Mouse.GetState();

            HasClicked = mouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released;
            MousePosition = mouseState.Position.ToVector2();

            _lastMouseState = mouseState;
        }
    }
}
