using Microsoft.Xna.Framework;
using MiFiSy_TPI.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MiFiSy_TPI.GameElement
{
    public class Home
    {
        private Button _btnPlay;
        private Dictionary<string, Button> _lstReplay;
        private List<Button> _lstBtnMusic;

        public Home()
        {
            _btnPlay = new Button(new Vector2(0.3f, 0.5f), 0.17f, 0.05f, "Commencer en mode libre", Color.Gray, Color.White, "play");
            _lstReplay = new Dictionary<string, Button>();
            _lstBtnMusic = new List<Button>();

            if (Directory.Exists(Config.PATH_SAVE_SEQUENCE))
            {
                string[] allReplay = Directory.GetFiles(Config.PATH_SAVE_SEQUENCE);
                for (int i = 1; i <= allReplay.Length; i++)
                {
                    string nameSequence = XDocument.Load(allReplay[i - 1]).Descendants("FireworkSequence").FirstOrDefault().Attribute("name").Value;
                    _lstReplay.Add(allReplay[i - 1], new Button(new Vector2(0.8f, Globals.ScreenHeight / (float)(allReplay.Length + 1) * i / Globals.ScreenHeight), 0.1f, 0.05f, nameSequence, Color.Gray, Color.White, "playReplay"));
                }
            }

            if (Directory.Exists(Config.PATH_MUSIC))
            {
                string[] allMusic = Directory.GetFiles(Config.PATH_MUSIC);
                for (int i = 1; i <= allMusic.Length; i++)
                {
                    string fileName = allMusic[i - 1].Split('/')[1];
                    _lstBtnMusic.Add(new Button(new Vector2(0.1f, Globals.ScreenHeight / (float)(allMusic.Length + 1) * i / Globals.ScreenHeight), 0.1f, 0.05f, fileName, Color.Gray, Color.White, "addMusic"));
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
