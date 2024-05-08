using Microsoft.Xna.Framework;
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
 * Description de la page : Classe de l'accueil
 */
namespace MiFiSy_TPI.Manager
{
    public class Home
    {
        private Button _btnPlay;
        private Dictionary<string, Button> _lstReplay;
        private List<Button> _lstBtnMusic;

        private const int NB_FILE_MAX = 10;

        /// <summary>
        /// Constructeur de la classe, récupère les musiques, les séquences sauvegardés des dossiers défini dans le fichier de configuration
        /// </summary>
        public Home()
        {
            _btnPlay = new Button(new Vector2(0.3f, 0.5f), 0.17f, 0.05f, "Commencer en mode libre", Color.Gray, Color.White, "play");
            _lstReplay = new Dictionary<string, Button>();
            _lstBtnMusic = new List<Button>();

            if (Directory.Exists(Config.PATH_SAVE_SEQUENCE))
            {
                string[] allReplay = Directory.GetFiles(Config.PATH_SAVE_SEQUENCE);
                if (allReplay.Length > NB_FILE_MAX)
                {
                    Array.Resize(ref allReplay, NB_FILE_MAX);
                }

                for (int i = 0; i < allReplay.Length; i++)
                {
                    string fileType = allReplay[i].Split(".")[1];
                    if (fileType == "xml")
                    {
                        string nameSequence = XDocument.Load(allReplay[i]).Descendants("FireworkSequence").FirstOrDefault().Attribute("name").Value;
                        _lstReplay.Add(allReplay[i], new Button(new Vector2(0.8f, Globals.ScreenHeight / (float)(allReplay.Length + 1) * (i + 1) / Globals.ScreenHeight), 0.1f, 0.05f, nameSequence, Color.Gray, Color.White, "playReplay"));
                    }
                }
            }

            if (Directory.Exists(Config.PATH_MUSIC))
            {
                string[] allMusic = Directory.GetFiles(Config.PATH_MUSIC);
                if (allMusic.Length > NB_FILE_MAX)
                {
                    Array.Resize(ref allMusic, NB_FILE_MAX);
                }

                for (int i = 0; i < allMusic.Length; i++)
                {
                    string fileType = allMusic[i].Split(".")[1];
                    if (fileType == "mp3" || fileType == "wav")
                    {
                        string fileName = allMusic[i].Split('/')[1];
                        _lstBtnMusic.Add(new Button(new Vector2(0.1f, Globals.ScreenHeight / (float)(allMusic.Length + 1) * (i + 1) / Globals.ScreenHeight), 0.1f, 0.05f, fileName, Color.Gray, Color.White, "addMusic"));
                    }
                }
            }
        }

        public void Update()
        {
            _btnPlay.Update();
            foreach (Button btnMusic in _lstBtnMusic)
            {
                btnMusic.Update();
                if (btnMusic.IsPressed)
                {
                    bool changeOk = true;
                    if (Globals.MusicSelectedName != "")
                    {
                        Button btnSelected = _lstBtnMusic.Find(x => x.Text == Globals.MusicSelectedName);
                        btnSelected.TextColor = Color.White;
                        Globals.MusicSelectedName = "";
                        if (btnSelected.Text == btnMusic.Text)
                        {
                            changeOk = false;
                        }
                    }

                    // Change la couleur en rouge si c'est une nouvelle musique qui est séléctionné
                    if (changeOk)
                    {
                        Globals.MusicSelectedName = btnMusic.Text;
                        btnMusic.TextColor = Color.Red;
                    }
                }
            }

            for (int i = 0; i < _lstReplay.Count; i++)
            {
                Button btn = _lstReplay.ElementAt(i).Value;
                btn.Update();
                if (btn.IsPressed)
                {
                    Globals.GameManager = new GameManager(false, "", _lstReplay.ElementAt(i).Key);
                    Globals.ActualPage = Globals.AllPage.Game;
                }
            }
        }

        /// <summary>
        /// Affiche tous les éléments de l'accueil
        /// </summary>
        public void Draw()
        {
            _btnPlay.Draw();
            Globals.SpriteBatch.DrawString(Globals.FontButton, "Choisir une musique : (optionnel)", new Vector2(_lstBtnMusic[0].Rectangle.X, _lstBtnMusic[0].Rectangle.Y - 50), Color.White);
            _lstBtnMusic.ForEach(x => x.Draw());
            if (_lstReplay.Count != 0)
            {
                Globals.SpriteBatch.DrawString(Globals.FontButton, "Revoir :", new Vector2(_lstReplay.ElementAt(0).Value.Rectangle.X, _lstReplay.ElementAt(0).Value.Rectangle.Y - 50), Color.White);
            }
            foreach (Button btn in _lstReplay.Values)
            {
                btn.Draw();
            }
        }
    }
}
