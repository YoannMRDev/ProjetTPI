using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
/*
 * Auteur : Yoann Meier
 * Date : 15/05/2024
 * Projet : Projet TPI, application de simulation de feux d'artifices en 2D
 * Description de la page : Classe pour gérer la souris
 */
namespace MiFiSy_TPI.Manager
{
    internal class InputManager
    {
        private static MouseState _lastMouseState;
        private static bool _hasSpaceKeyUp;
        private static bool _hasEnterKeyUp;

        public static bool HasClicked { get; private set; }
        public static Vector2 MousePosition { get; private set; }
        public static bool IsKeyParticleRainPressed { get; private set; }
        public static bool IsKeyCometPressed { get; private set; }

        public static void Update()
        {
            // Souris
            var mouseState = Mouse.GetState();

            HasClicked = mouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released;
            MousePosition = mouseState.Position.ToVector2();
            _lastMouseState = mouseState;

            // Espace
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                _hasSpaceKeyUp = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && _hasSpaceKeyUp)
            {
                IsKeyParticleRainPressed = true;
                _hasSpaceKeyUp = false;
            }
            else
            {
                IsKeyParticleRainPressed = false;
            }

            // Enter
            if (Keyboard.GetState().IsKeyUp(Keys.Enter))
            {
                _hasEnterKeyUp = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && _hasEnterKeyUp)
            {
                IsKeyCometPressed = true;
                _hasEnterKeyUp = false;
            }
            else
            {
                IsKeyCometPressed = false;
            }
        }
    }
}
