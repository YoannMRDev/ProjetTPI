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
        private List<Button> _lstBtnReplay;
        private List<Button> _lstBtnMusic;

        public Home()
        {
            _btnPlay = new Button(new Vector2(0.3f, 0.5f), 0.1f, 0.05f, "Libre", Color.Gray, Color.White, "play");
            _lstBtnReplay = new List<Button>();
            _lstBtnMusic = new List<Button>();

            if (Directory.Exists(Config.PATH_SAVE_SEQUENCE))
            {
                string[] allReplay = Directory.GetFiles(Config.PATH_SAVE_SEQUENCE);
                for (int i = 1; i <= allReplay.Length; i++)
                {
                    string nameSequence = XDocument.Load(allReplay[i - 1]).Descendants("FireworkSequence").FirstOrDefault().Attribute("name").Value;
                    _lstBtnReplay.Add(new Button(new Vector2(0.8f, Globals.ScreenHeight / (float)(allReplay.Length + 1) * i / Globals.ScreenHeight), 0.1f, 0.05f, nameSequence, Color.Gray, Color.White, "playReplay"));
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
                    if (Globals.MusicSelectedName != "")
                    {
                        _lstBtnMusic.Find(x => x.Text == Globals.MusicSelectedName).TextColor = Color.White;
                    }
                    Globals.MusicSelectedName = btnMusic.Text;
                    btnMusic.TextColor = Color.Red;
                }
            }
            foreach (Button btnReplay in _lstBtnReplay)
            {
                btnReplay.Update();
                if (btnReplay.IsPressed)
                {
                    Globals.GameManager = new GameManager(false, "", btnReplay.Text);
                    Globals.ActualPage = Globals.AllPage.Game;
                }
            }
        }

        public void Draw()
        {
            _btnPlay.Draw();
            Globals.SpriteBatch.DrawString(Globals.FontButton, "Choisir une musique : (optionnel)", new Vector2(_lstBtnMusic[0].Rectangle.X, _lstBtnMusic[0].Rectangle.Y - 50), Color.White);
            _lstBtnMusic.ForEach(x => x.Draw());
            Globals.SpriteBatch.DrawString(Globals.FontButton, "Revoir :", new Vector2(_lstBtnReplay[0].Rectangle.X, _lstBtnReplay[0].Rectangle.Y - 50), Color.White);
            _lstBtnReplay.ForEach(x => x.Draw());
        }
    }
}
