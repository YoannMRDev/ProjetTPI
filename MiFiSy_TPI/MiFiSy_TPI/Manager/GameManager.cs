using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MiFiSy_TPI.Firework;
using MiFiSy_TPI.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
/*
 * Auteur : Yoann Meier
 * Date : 06/05/2024
 * Projet : Projet TPI, application de simulation de feux d'artifices en 2D
 * Description de la page : Classe de gestion du jeu libre et replay
 */
namespace MiFiSy_TPI.Manager
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
        private XElement _file;

        // Message après sauvegarde
        private float _timerSave;
        private bool showMessageSave;

        private const float TIME_MESSAGE_SAVE = 2f;

        // Constantes des noms d'éléments et d'attributs pour la sauvegarde et la récupération des données lors du replay
        private const string ATTRIBUTE_NAME = "name";
        private const string ATTRIBUTE_CREATION_DATE = "creationDate";
        private const string ATTRIBUTE_AUTHOR = "author";
        private const string ATTRIBUTE_TIME_END = "timeEnd";
        private const string ATTRIBUTE_POSITION_X = "positionX";
        private const string ATTRIBUTE_POSITION_Y = "positionY";
        private const string ATTRIBUTE_WIDTH = "width";
        private const string ATTRIBUTE_HEIGHT = "height";
        private const string ATTRIBUTE_ANGLE = "angle";
        private const string ATTRIBUTE_SPEED = "speed";
        private const string ATTRIBUTE_LIFESPAN = "lifeSpan";
        private const string ATTRIBUTE_NB_PARTICLE = "nbParticle";
        private const string ATTRIBUTE_MAIN_SIZE = "main";
        private const string ATTRIBUTE_OTHER_SIZE = "other";
        private const string ELEMENT_MORTAR = "mortar";
        private const string ELEMENT_START = "start";
        private const string ELEMENT_SIZE = "Size";
        private const string ELEMENT_AUDIO = "Audio";
        private const string ELEMENT_BACKGROUND = "Background";
        private const string ELEMENT_FIREWORK_SEQUENCE = "FireworkSequence";
        private const string ELEMENT_FIREWORK = "Firework";
        private const string ELEMENT_COLOR_START = "ColorStart";
        private const string ELEMENT_COLOR_END = "ColorEnd";
        private const string ATTRIBUTE_R_COLOR = "r";
        private const string ATTRIBUTE_G_COLOR = "g";
        private const string ATTRIBUTE_B_COLOR = "b";
        private const string ATTRIBUTE_TRACK = "track";
        private const string ATTRIBUTE_IMG = "img";
        private const string ATTRIBUTE_LAUNCH_TIME = "launchTime";
        private const string ATTRIBUTE_TYPE_COMET = "Comet";
        private const string ATTRIBUTE_TYPE_PARTICLE_RAIN = "ParticleRain";
        private const string ATTRIBUTE_TYPE = "type";

        public bool Mode { get => _mode; set => _mode = value; }

        /// <summary>
        /// Constructeur de la classe
        /// </summary>
        /// <param name="mode">Si mode = true, on est dans le mode libre, si mode = false, on est dans le mode replay</param>
        /// <param name="musiqueName">nom de la musique, optionnel</param>
        /// <param name="replayFileName">nom du replay, obligatoire dans le mode replay</param>
        public GameManager(bool mode, string musiqueName = "", string replayFileName = "")
        {
            Mode = mode;
            _lstMortar = new List<Mortar>();
            _timerLauch = 0;
            _timerSave = 0;
            showMessageSave = false;

            _menuButton = new Button(new Vector2(0.01f, 0.01f), 0.1f, 0.05f, "Accueil", Color.Gray, Color.White, "goBack");

            if (Mode)
            {
                _saveButton = new Button(new Vector2(0.89f, 0.01f), 0.1f, 0.05f, "Sauvegarder", Color.Gray, Color.White, "save");

                // Charge et lance la musique si une musique a été choisi
                if (musiqueName != "")
                {
                    try
                    {
                        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Config.PATH_MUSIC, musiqueName);
                        _music = Song.FromUri(Path.GetFileName(fullPath), new Uri(fullPath));
                        MediaPlayer.Play(_music);
                    }
                    catch { /* le fichier n'existe plus ou ce n'est pas une musique */ }
                }

                // Charge l'image si un chemin est indiqué dans le fichier de configuration
                if (Config.PATH_IMG != "")
                {
                    try
                    {
                        _background = Texture2D.FromFile(Globals.GraphicsDevice, Config.PATH_IMG);
                    }
                    catch { /* Le fichier n'existe pas ou n'est pas un format image */ }
                }

                // Ajoute tous les mortiers spécifiés dans le fichier de configuration
                if (Config.ALL_MORTAR.Count != 0)
                {
                    foreach (XElement mortar in Config.ALL_MORTAR)
                    {
                        AddMortarFromXElementToListMortar(mortar);
                    }
                }
                // Sinon, ajoute 5 mortiers par défaut
                else
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        // (float)(5 + 1) : le float sert à ne pas arrondir à 0
                        _lstMortar.Add(new Mortar(new Vector2(Globals.ScreenWidth / (float)(5 + 1) * i / Globals.ScreenWidth, 1 - 0.15f), 0.025f, 0.15f, 10, Color.White));
                    }
                }
            }
            else
            {
                // Charge le fichier pour le rejouer
                _file = XDocument.Load(replayFileName).Descendants(ELEMENT_FIREWORK_SEQUENCE).FirstOrDefault();

                // Créer tous les mortiers
                foreach (XElement mortar in _file.Descendants(ELEMENT_MORTAR))
                {
                    AddMortarFromXElementToListMortar(mortar);
                }

                // Charge l'image si un chemin est indiqué dans le fichier de la séquence
                if (_file.Descendants(ELEMENT_BACKGROUND).Attributes(ATTRIBUTE_IMG).FirstOrDefault().Value != "")
                {
                    try
                    {
                        _background = Texture2D.FromFile(Globals.GraphicsDevice, _file.Descendants(ELEMENT_BACKGROUND).Attributes(ATTRIBUTE_IMG).FirstOrDefault().Value);
                    }
                    catch { /* Le fichier n'existe pas ou n'est pas un format image */ }
                }

                // Charge et lance la musique si elle est indiqué dans le fichier de la séquence
                if (_file.Descendants(ELEMENT_AUDIO).Attributes(ATTRIBUTE_TRACK).FirstOrDefault().Value != "")
                {
                    try
                    {
                        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _file.Descendants(ELEMENT_AUDIO).Attributes(ATTRIBUTE_TRACK).FirstOrDefault().Value);
                        _music = Song.FromUri(Path.GetFileName(fullPath), new Uri(fullPath));
                        MediaPlayer.Play(_music);
                    }
                    catch { /* le fichier n'existe plus ou ce n'est pas une musique */ }

                }
            }
        }

        /// <summary>
        /// Ajoute dans la liste de mortier un élément récupéré de fichier xml
        /// </summary>
        /// <param name="mortar"></param>
        public void AddMortarFromXElementToListMortar(XElement mortar)
        {
            _lstMortar.Add(new Mortar(new Vector2(float.Parse(mortar.Attribute(ATTRIBUTE_POSITION_X).Value), float.Parse(mortar.Attribute(ATTRIBUTE_POSITION_Y).Value)), float.Parse(mortar.Attribute(ATTRIBUTE_WIDTH).Value),
                float.Parse(mortar.Attribute(ATTRIBUTE_HEIGHT).Value), float.Parse(mortar.Attribute(ATTRIBUTE_ANGLE).Value), Color.White));
        }

        /// <summary>
        /// Sauvegarde toute la séquence en XML
        /// </summary>
        public void SaveSequence()
        {
            string musicName = Globals.MusicSelectedName != "" ? Config.PATH_MUSIC + Globals.MusicSelectedName : string.Empty;
            // Information global de la séquence, nom, auteur, date...
            DateTime currentDate = DateTime.Now;
            XDocument document = new XDocument(
                new XElement(ELEMENT_FIREWORK_SEQUENCE,
                    new XAttribute(ATTRIBUTE_NAME, Config.NAME_SEQUENCE),
                    new XAttribute(ATTRIBUTE_CREATION_DATE, currentDate.ToString("yyyy-MM-dd")),
                    new XAttribute(ATTRIBUTE_AUTHOR, Config.AUTHOR_FILE),
                    new XAttribute(ATTRIBUTE_TIME_END, _timerLauch.ToString().Replace(".", ",")),
                    new XElement(ELEMENT_AUDIO,
                        new XAttribute(ATTRIBUTE_TRACK, musicName)
                    ),
                    new XElement(ELEMENT_BACKGROUND,
                        new XAttribute(ATTRIBUTE_IMG, Config.PATH_IMG)
                    )
                )
            );
            XElement fireworkSequence = document.Descendants(ELEMENT_FIREWORK_SEQUENCE).FirstOrDefault();

            // Ajoute les informations des mortiers
            foreach (Mortar mortar in _lstMortar)
            {
                fireworkSequence.Add(
                    new XElement(ELEMENT_MORTAR,
                        new XAttribute(ATTRIBUTE_POSITION_X, mortar.Position.X.ToString().Replace(".", ",")),
                        new XAttribute(ATTRIBUTE_POSITION_Y, mortar.Position.Y.ToString().Replace(".", ",")),
                        new XAttribute(ATTRIBUTE_WIDTH, mortar.Width.ToString().Replace(".", ",")),
                        new XAttribute(ATTRIBUTE_HEIGHT, mortar.Height.ToString().Replace(".", ",")),
                        new XAttribute(ATTRIBUTE_ANGLE, MathHelper.ToDegrees(mortar.Angle).ToString().Replace(".", ","))
                    )
                );
            }

            // Ajoute les feux d'artifices
            foreach (IFirework firework in Globals.LstFirework)
            {
                if (firework is Comet comet)
                {
                    XElement cometElement = CreateCommonFireworkElement(comet, ATTRIBUTE_TYPE_COMET);
                    cometElement.Add(
                        new XElement(ELEMENT_SIZE,
                            new XAttribute(ATTRIBUTE_MAIN_SIZE, Config.COMET_MAIN_SIZE),
                            new XAttribute(ATTRIBUTE_OTHER_SIZE, Config.COMET_OTHER_SIZE)
                        ),
                        new XElement(ELEMENT_START,
                            new XAttribute(ATTRIBUTE_POSITION_X, comet.StartPosition.X.ToString().Replace(".", ",")),
                            new XAttribute(ATTRIBUTE_POSITION_Y, comet.StartPosition.Y.ToString().Replace(".", ",")),
                            new XAttribute(ATTRIBUTE_ANGLE, comet.StartAngle.ToString().Replace(".", ",")),
                            new XAttribute(ATTRIBUTE_SPEED, comet.StartSpeed.ToString().Replace(".", ",")),
                            new XAttribute(ATTRIBUTE_LIFESPAN, comet.Lifespan.ToString().Replace(".", ","))
                        )
                    );
                    fireworkSequence.Add(cometElement);
                }
                else if (firework is ParticleRain rain)
                {
                    XElement rainElement = CreateCommonFireworkElement(rain, ATTRIBUTE_TYPE_PARTICLE_RAIN);
                    rainElement.Add(
                        new XElement(ELEMENT_SIZE, Config.PARTICLE_RAIN_SIZE),
                        new XElement(ELEMENT_START,
                            new XAttribute(ATTRIBUTE_POSITION_X, rain.StartPosition.X.ToString().Replace(".", ",")),
                            new XAttribute(ATTRIBUTE_POSITION_Y, rain.StartPosition.Y.ToString().Replace(".", ",")),
                            new XAttribute(ATTRIBUTE_SPEED, rain.StartSpeed.ToString().Replace(".", ",")),
                            new XAttribute(ATTRIBUTE_LIFESPAN, rain.Lifespan.ToString().Replace(".", ",")),
                            new XAttribute(ATTRIBUTE_NB_PARTICLE, Config.PARTICLE_RAIN_NB)
                        )
                    );
                    fireworkSequence.Add(rainElement);
                }
            }
            // Sauvegarde le fichier
            document.Save($"{Config.PATH_SAVE_SEQUENCE}{currentDate.ToString("yyyy-MM-dd HH_mm_ss")}.xml");
            Globals.LstFirework.Clear();
            _timerLauch = 0f;
        }

        /// <summary>
        /// Crée les éléments communs au feu d'artifice comme la couleur et le temps de lancement
        /// </summary>
        /// <param name="firework">feu d'artifice lancé, pour récupéré le launchTime</param>
        /// <param name="type">type de feu d'artifice : Comète, pluie de particule</param>
        private static XElement CreateCommonFireworkElement(IFirework firework, string type)
        {
            XElement baseParticle = new XElement(ELEMENT_FIREWORK,
                new XAttribute(ATTRIBUTE_TYPE, type),
                new XAttribute(ATTRIBUTE_LAUNCH_TIME, firework.LaunchTime.ToString().Replace(".", ","))
           );
            if (type == ATTRIBUTE_TYPE_COMET)
            {
                baseParticle.Add(
                    new XElement(ELEMENT_COLOR_START,
                        new XAttribute(ATTRIBUTE_R_COLOR, Config.COLOR_START_COMET.R),
                        new XAttribute(ATTRIBUTE_G_COLOR, Config.COLOR_START_COMET.G),
                        new XAttribute(ATTRIBUTE_B_COLOR, Config.COLOR_START_COMET.B)
                    ),
                     new XElement(ELEMENT_COLOR_END,
                         new XAttribute(ATTRIBUTE_R_COLOR, Config.COLOR_END_COMET.R),
                         new XAttribute(ATTRIBUTE_G_COLOR, Config.COLOR_END_COMET.G),
                         new XAttribute(ATTRIBUTE_B_COLOR, Config.COLOR_END_COMET.B)
                     )
                );
            }
            else if (type == ATTRIBUTE_TYPE_PARTICLE_RAIN)
            {
                baseParticle.Add(
                    new XElement(ELEMENT_COLOR_START,
                        new XAttribute(ATTRIBUTE_R_COLOR, Config.COLOR_PARTICLE_RAIN_START.R),
                        new XAttribute(ATTRIBUTE_G_COLOR, Config.COLOR_PARTICLE_RAIN_START.G),
                        new XAttribute(ATTRIBUTE_B_COLOR, Config.COLOR_PARTICLE_RAIN_START.B)
                    ),
                     new XElement(ELEMENT_COLOR_END,
                         new XAttribute(ATTRIBUTE_R_COLOR, Config.COLOR_PARTICLE_RAIN_END.R),
                         new XAttribute(ATTRIBUTE_G_COLOR, Config.COLOR_PARTICLE_RAIN_END.G),
                         new XAttribute(ATTRIBUTE_B_COLOR, Config.COLOR_PARTICLE_RAIN_END.B)
                     )
                );
            }
            return baseParticle;
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

        public void Update()
        {
            _timerLauch += Globals.TotalSeconds;

            _menuButton.Update();
            try
            {
                Globals.LstFirework.ForEach(x => x.Update());
            }
            catch (InvalidOperationException) { /* Il arrive parfois qu'un feu d'artifice soit ajouté pendant la mise à jour */ }

            if (Mode)
            {
                _saveButton.Update();

                if (_saveButton.IsPressed)
                {
                    SaveSequence();
                    showMessageSave = true;
                }

                // permet d'afficher le message de confirmation de sauvegarde pendant un certain temps
                if (showMessageSave)
                {
                    _timerSave += Globals.TotalSeconds;

                    if (_timerSave >= TIME_MESSAGE_SAVE)
                    {
                        _timerSave = 0;
                        showMessageSave = false;
                    }
                }

                // permet d'utiliser les touches du clavier au cas où la guitare ne fonctionne pas lors de la présentation
                if (InputManager.IsKeyParticleRainPressed)
                {
                    Globals.LstFirework.Add(new ParticleRain(80, 3f, _timerLauch));
                }
                if (InputManager.IsKeyCometPressed)
                {
                    int nbMortar = Globals.RandomInt(0, Config.ALL_MORTAR.Count - 1);
                    Vector2 emitPos = _lstMortar[nbMortar].Position;
                    emitPos.X += _lstMortar[nbMortar].Width / 2;
                    Globals.LstFirework.Add(new Comet(emitPos, _lstMortar[nbMortar].Angle, 400, 1.5f, _timerLauch));
                }
            }
            else
            {
                // Rejoue toute la séquence
                foreach (XElement firework in _file.Descendants(ELEMENT_FIREWORK))
                {
                    if (float.Parse(firework.Attribute(ATTRIBUTE_LAUNCH_TIME).Value) == _timerLauch)
                    {
                        // Récupère les informations communs aux feux d'artifices
                        Color colorStart = Globals.GetColorFromElement(firework.Descendants(ELEMENT_COLOR_START).FirstOrDefault());
                        Color colorEnd = Globals.GetColorFromElement(firework.Descendants(ELEMENT_COLOR_END).FirstOrDefault());
                        float positionX = float.Parse(firework.Descendants(ELEMENT_START).FirstOrDefault().Attribute(ATTRIBUTE_POSITION_X).Value);
                        float positionY = float.Parse(firework.Descendants(ELEMENT_START).FirstOrDefault().Attribute(ATTRIBUTE_POSITION_Y).Value);
                        float speed = float.Parse(firework.Descendants(ELEMENT_START).FirstOrDefault().Attribute(ATTRIBUTE_SPEED).Value);
                        float lifespan = float.Parse(firework.Descendants(ELEMENT_START).FirstOrDefault().Attribute(ATTRIBUTE_LIFESPAN).Value);

                        string fireworkType = firework.Attribute(ATTRIBUTE_TYPE).Value;
                        if (fireworkType == ATTRIBUTE_TYPE_COMET)
                        {
                            // Crée une comète
                            float sizeMain = float.Parse(firework.Descendants(ELEMENT_SIZE).FirstOrDefault().Attribute(ATTRIBUTE_MAIN_SIZE).Value);
                            float sizeOther = float.Parse(firework.Descendants(ELEMENT_SIZE).FirstOrDefault().Attribute(ATTRIBUTE_OTHER_SIZE).Value);
                            float angle = float.Parse(firework.Descendants(ELEMENT_START).FirstOrDefault().Attribute(ATTRIBUTE_ANGLE).Value);
                            Globals.LstFirework.Add(new Comet(new Vector2(positionX, positionY), angle, speed, lifespan, colorStart, colorEnd, sizeMain, sizeOther));
                        }
                        else if (fireworkType == ATTRIBUTE_TYPE_PARTICLE_RAIN)
                        {
                            // Créer une pluie de particule
                            float size = float.Parse(firework.Descendants(ELEMENT_SIZE).FirstOrDefault().Value);
                            float nbParticle = float.Parse(firework.Descendants(ELEMENT_START).FirstOrDefault().Attribute(ATTRIBUTE_NB_PARTICLE).Value);
                            Globals.LstFirework.Add(new ParticleRain(new Vector2(positionX, positionY), speed, lifespan, colorStart, colorEnd, size, nbParticle));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Méthode d'affichage du jeu, libre et replay
        /// </summary>
        public void Draw()
        {
            if (Mode)
            {
                // Affiche l'image de fond si elle a été spécifiée dans le fichier de configuration
                if (_background != null)
                {
                    Globals.SpriteBatch.Draw(_background, new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight), Color.White);
                }
                _saveButton.Draw();

                // Affiche le message de confirmation de sauvegarde
                if (showMessageSave)
                {
                    Globals.SpriteBatch.DrawString(Globals.FontButton, "Sauvegarde effectue", new Vector2(0.5f * Globals.ScreenWidth, 0.5f * Globals.ScreenHeight), Color.Red);
                }
            }
            else
            {
                // Affiche l'image de fond si elle a été spécifiée dans le fichier de la séquence
                if (_background != null)
                {
                    Globals.SpriteBatch.Draw(_background, new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight), Color.White);
                }

                // Affiche les données du replay
                Globals.SpriteBatch.DrawString(Globals.FontButton, $"Nom de la sequence : {_file.Attribute(ATTRIBUTE_NAME).Value}", new Vector2(0.75f * Globals.ScreenWidth, 0.05f * Globals.ScreenHeight), Color.White);
                Globals.SpriteBatch.DrawString(Globals.FontButton, $"Auteur : {_file.Attribute(ATTRIBUTE_AUTHOR).Value}", new Vector2(0.75f * Globals.ScreenWidth, 0.1f * Globals.ScreenHeight), Color.White);
                Globals.SpriteBatch.DrawString(Globals.FontButton, $"Date : {_file.Attribute(ATTRIBUTE_CREATION_DATE).Value}", new Vector2(0.75f * Globals.ScreenWidth, 0.15f * Globals.ScreenHeight), Color.White);

                // Affiche un message de fin de replay
                if (_timerLauch >= float.Parse(_file.Attribute(ATTRIBUTE_TIME_END).Value))
                {
                    Globals.SpriteBatch.DrawString(Globals.FontButton, "Fin du replay", new Vector2(0.5f * Globals.ScreenWidth, 0.5f * Globals.ScreenHeight), Color.Red);
                }
            }

            _menuButton.Draw();
            _lstMortar.ForEach(m => m.Draw());
        }
    }
}
