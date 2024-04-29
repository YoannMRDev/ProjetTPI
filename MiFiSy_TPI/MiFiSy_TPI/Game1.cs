using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiFiSy_TPI.GameElement;
using MiFiSy_TPI.GameElement.Firework;
using MiFiSy_TPI.ParticleCreator;
using System.Collections.Generic;

namespace MiFiSy_TPI
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameManager _gameManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            Globals.ScreenWidth = _graphics.PreferredBackBufferWidth;
            Globals.ScreenHeight = _graphics.PreferredBackBufferHeight;
            Globals.Content = Content;
            Globals.GraphicsDevice = GraphicsDevice;
            Globals.ActualPage = Globals.AllPage.Jeu;
            Globals.LstComete = new List<Comet>();
            Globals.LstParticleRain = new List<ParticleRain>();

            Globals.FontButton = Content.Load<SpriteFont>("Font/fontButton");

            new JamstikMidiListener();
            _gameManager = new GameManager(true);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.SpriteBatch = _spriteBatch;
        }

        protected override void Update(GameTime gameTime)
        {
            Globals.Update(gameTime);
            InputManager.Update();
            switch (Globals.ActualPage)
            {
                case Globals.AllPage.Accueil:
                    break;
                case Globals.AllPage.Jeu:
                    ParticleManager.Update();
                    _gameManager.Update();
                    break;
                default:
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            switch (Globals.ActualPage)
            {
                case Globals.AllPage.Accueil:
                    break;
                case Globals.AllPage.Jeu:
                    ParticleManager.Draw();
                    _gameManager.Draw();
                    break;
                default:
                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
