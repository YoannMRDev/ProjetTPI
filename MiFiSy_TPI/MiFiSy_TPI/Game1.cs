using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MiFiSy_TPI.GameElement;
using MiFiSy_TPI.GameElement.Firework;
using MiFiSy_TPI.ParticleCreator;
using System.Collections.Generic;
/*
 * Auteur : Yoann Meier
 * Date : 06/05/2024
 * Projet : Projet TPI, application de simulation de feux d'artifices en 2D
 * Description de la page : Page principale de l'application
 */
namespace MiFiSy_TPI
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private JamstikMidiListener _jamstikMidiListener;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // taille de l'application en fonction de la taille de l'écran
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            Globals.ScreenWidth = _graphics.PreferredBackBufferWidth;
            Globals.ScreenHeight = _graphics.PreferredBackBufferHeight;
            Globals.Content = Content;
            Globals.GraphicsDevice = GraphicsDevice;
            Globals.ActualPage = Globals.AllPage.Home;
            Globals.LstFirework = new List<IFirework>();
            Globals.MusicSelectedName = "";
            Globals.FontButton = Content.Load<SpriteFont>("Font/fontButton");

            // Permet de mettre en boucle les musiques
            MediaPlayer.IsRepeating = true;

            new Config();
            Globals.GameManager = new GameManager(true);
            Globals.home = new Home();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.SpriteBatch = _spriteBatch;
            _jamstikMidiListener = new JamstikMidiListener(Content.Load<SpriteFont>("Font/fontErrorMidi"));
        }

        protected override void Update(GameTime gameTime)
        {
            Globals.Update(gameTime);
            InputManager.Update();
            switch (Globals.ActualPage)
            {
                // Page d'accueil
                case Globals.AllPage.Home:
                    Globals.home.Update();
                    break;
                // Page de jeu
                case Globals.AllPage.Game:
                    ParticleManager.Update();
                    Globals.GameManager.Update();
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Globals.SpriteBatch.Begin();
            switch (Globals.ActualPage)
            {
                // Page d'accueil
                case Globals.AllPage.Home:
                    _jamstikMidiListener.DrawErrorNotConnected();
                    Globals.home.Draw();
                    break;
                // Page de jeu
                case Globals.AllPage.Game:
                    Globals.GameManager.Draw();
                    ParticleManager.Draw();
                    break;
            }
            Globals.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
