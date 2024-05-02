using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MiFiSy_TPI.GameElement.Firework;
using MiFiSy_TPI.ParticleCreator;
using MiFiSy_TPI.UI;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MiFiSy_TPI.GameElement
{
    internal class GameManager
    {
        private bool _mode;
        private Button _menuButton;
        private Button _saveButton;
        private List<Mortar> _lstMortar;
        private float _timerLauch;
        private Song _music;
        private Texture2D _background;

        // Message après sauvegarde
        private float _timerSave;
        private bool showMessageSave;

        private const float TIME_MESSAGE_SAVE = 2f;

        public bool Mode { get => _mode; set => _mode = value; }

        public GameManager(bool mode, string musiqueName = "", string replayName = "")
        {
            // Si mode = true, on est dans le mode libre, si mode = false, on est dans le mode replay
            Mode = mode;
            _lstMortar = new List<Mortar>();
            _timerLauch = 0;
            _timerSave = 0;
            showMessageSave = false;
            if (Mode)
            {
                if (musiqueName != "")
                {
                    // Charge la musique sans Content et joue la musique
                    string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Config.PATH_MUSIC, musiqueName);
                    _music = Song.FromUri(Path.GetFileName(fullPath), new Uri(fullPath));
                    MediaPlayer.Play(_music);
                }
                _saveButton = new Button(new Vector2(0.89f, 0.01f), 0.1f, 0.05f, "Sauvegarder", Color.Gray, Color.White, "save");

                if (Config.PATH_IMG != "")
                {
                    _background = Texture2D.FromFile(Globals.GraphicsDevice, Config.PATH_IMG);
                }
            }
            else
            {
                // Charge le replay
            }
            _menuButton = new Button(new Vector2(0.01f, 0.01f), 0.1f, 0.05f, "Accueil", Color.Gray, Color.White, "goBack");

            if (Config.ALL_MORTAR.Count != 0)
            {
                foreach (XElement mortar in Config.ALL_MORTAR)
                {
                    _lstMortar.Add(new Mortar(new Vector2(float.Parse(mortar.Attribute("positionX").Value), float.Parse(mortar.Attribute("positionY").Value)), float.Parse(mortar.Attribute("width").Value), float.Parse(mortar.Attribute("height").Value),
                        float.Parse(mortar.Attribute("angle").Value), Color.White));
                }
            }
            else
            {
                // Crée 5 mortier par défaut si il n'y a rien dans le fichier config.xml
                for (int i = 1; i <= 5; i++)
                {
                    // (float)(5 + 1) : le float sert à ne pas arrondir à 0
                    _lstMortar.Add(new Mortar(new Vector2(Globals.ScreenWidth / (float)(5 + 1) * i / Globals.ScreenWidth, 1 - 0.15f), 0.025f, 0.15f, 10, Color.White));
                }
            }
        }

        public void Update()
        {
            _menuButton.Update();
            if (Mode)
            {
                _timerLauch += Globals.TotalSeconds;
                try
                {
                    Globals.LstFirework.ForEach(x => x.Update());

                }
                catch (InvalidOperationException)
                {
                    
                }
                _saveButton.Update();

                if (_saveButton.IsPressed)
                {
                    SaveSequence();
                    showMessageSave = true;
                }

                if (showMessageSave)
                {
                    _timerSave += Globals.TotalSeconds;

                    if (_timerSave >= TIME_MESSAGE_SAVE)
                    {
                        _timerSave = 0;
                        showMessageSave = false;
                    }
                }


                // test (a supprimé)
                if (InputManager.HasClicked)
                {
                    int nbMortar = Globals.RandomInt(0, Config.ALL_MORTAR.Count - 1);
                    Vector2 emitPos = _lstMortar[nbMortar].Position;
                    emitPos.X += _lstMortar[nbMortar].Width / 2;
                    Globals.LstFirework.Add(new Comet(emitPos, _lstMortar[nbMortar].Angle, 400, 1.5f, _timerLauch));
                    Globals.LstFirework.Add(new ParticleRain(80, 1.5f, _timerLauch));
                }
            }
            else
            {
                // Update du replay
            }
        }

        /// <summary>
        /// Sauvegarde la séquence en XML
        /// </summary>
        public void SaveSequence()
        {
            DateTime currentDate = DateTime.Now;
            XDocument document = new XDocument(
                new XElement("FireworkSequence",
                    new XAttribute("name", Config.NAME_SEQUENCE),
                    new XAttribute("creationDate", currentDate.ToString("yyyy-MM-dd")),
                    new XAttribute("author", Config.AUTHOR_FILE),
                    new XElement("Audio",
                        new XAttribute("track", Globals.MusicSelectedName)
                    ),
                    new XElement("Background",
                        new XAttribute("img", Config.PATH_IMG)
                    )
                )
            );
            XElement fireworkSequence = document.Descendants("FireworkSequence").FirstOrDefault();
            // Ajoute les informations des mortiers
            foreach (Mortar mortar in _lstMortar)
            {
                fireworkSequence.Add(
                    new XElement("mortar",
                        new XAttribute("positionX", mortar.Position.X),
                        new XAttribute("positionY", mortar.Position.Y),
                        new XAttribute("width", mortar.Width),
                        new XAttribute("height", mortar.Height),
                        new XAttribute("angle", MathHelper.ToDegrees(mortar.Angle))
                    )
                );
            }
            // Ajoute les feu d'artifices
            foreach (IFirework firework in Globals.LstFirework)
            {
                if (firework is Comet comet)
                {
                    XElement cometElement = CreateCommonFireworkElement(comet, "Comet");
                    cometElement.Add(
                        new XElement("Size",
                            new XAttribute("main", Config.COMET_MAIN_SIZE),
                            new XAttribute("other", Config.COMET_OTHER_SIZE)
                        ),
                        new XElement("start",
                            new XAttribute("x", comet.StartPosition.X),
                            new XAttribute("y", comet.StartPosition.Y),
                            new XAttribute("angle", comet.StartAngle),
                            new XAttribute("speed", comet.StartSpeed),
                            new XAttribute("lifeSpan", comet.Lifespan)
                        )
                    );
                    fireworkSequence.Add(cometElement);
                }
                else if (firework is ParticleRain rain)
                {
                    XElement rainElement = CreateCommonFireworkElement(rain, "ParticleRain");
                    rainElement.Add(
                        new XElement("Size", Config.PARTICLE_RAIN_SIZE),
                        new XElement("start",
                            new XAttribute("x", rain.StartPosition.X),
                            new XAttribute("y", rain.StartPosition.Y),
                            new XAttribute("speed", rain.StartSpeed),
                            new XAttribute("lifeSpan", rain.Lifespan),
                            new XAttribute("nbParticle", Config.PARTICLE_RAIN_NB)
                        )
                    );
                    fireworkSequence.Add(rainElement);
                }
            }
            document.Save($"{Config.PATH_SAVE_SEQUENCE}{currentDate.ToString("yyyy-MM-dd HH_mm_ss")}.xml");
            Globals.LstFirework.Clear();
        }

        /// <summary>
        /// Crée les éléments communs au feu d'artifice
        /// </summary>
        /// <param name="firework"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static XElement CreateCommonFireworkElement(IFirework firework, string type)
        {
            return new XElement("Firework",
                new XAttribute("type", type),
                new XAttribute("lauchTime", firework.LaunchTime),
                new XElement("ColorStart",
                    new XAttribute("r", Config.COLOR_START.R),
                    new XAttribute("g", Config.COLOR_START.G),
                    new XAttribute("b", Config.COLOR_START.B)
                ),
                new XElement("ColorEnd",
                    new XAttribute("r", Config.COLOR_END.R),
                    new XAttribute("g", Config.COLOR_END.G),
                    new XAttribute("b", Config.COLOR_END.B)
                )
            );
        }

        /// <summary>
        /// Crée une comète
        /// </summary>
        /// <param name="velocity">La vitesse change en fonction de la vélocité</param>
        public void CreateComete(int velocity)
        {
            int nbMortar = Globals.RandomInt(0, Config.ALL_MORTAR.Count - 1);
            Vector2 emitPos = _lstMortar[nbMortar].Position;
            emitPos.X += _lstMortar[nbMortar].Width / 2;
            Globals.LstFirework.Add(new Comet(emitPos, _lstMortar[nbMortar].Angle, Config.COMET_DEFAULT_SPEED * velocity, Config.COMET_DEFAULT_LIFESPAN, _timerLauch));
        }

        /// <summary>
        /// Crée une pluie de particule
        /// </summary>
        /// <param name="velocity">La durée de vie change en fonction de la vélocité</param>
        public void CreateParticleRain(int velocity)
        {
            Globals.LstFirework.Add(new ParticleRain(Config.PARTICLE_RAIN_SPEED, Config.PARTICLE_RAIN_LIFESPAN * velocity, _timerLauch));
        }

        public void Draw()
        {
            if (Mode)
            {
                if (Config.PATH_IMG != "")
                {
                    Globals.SpriteBatch.Draw(_background, new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight), Color.White);
                }
                _saveButton.Draw();
                if (showMessageSave)
                {
                    Globals.SpriteBatch.DrawString(Globals.FontButton, "Sauvegarde effectue", new Vector2(0.5f * Globals.ScreenWidth, 0.5f * Globals.ScreenHeight), Color.Red);
                }
            }
            else
            {
                // Affiche le replay
                // Affiche les données du replay
            }
            _menuButton.Draw();
            _lstMortar.ForEach(m => m.Draw());
        }
    }
}
