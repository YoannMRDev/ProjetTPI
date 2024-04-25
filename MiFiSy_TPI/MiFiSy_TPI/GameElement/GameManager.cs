using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MiFiSy_TPI.GameElement.Firework;
using MiFiSy_TPI.ParticleCreator;
using MiFiSy_TPI.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiFiSy_TPI.GameElement
{
    internal class GameManager
    {
        private bool _mode;
        private Button _menuButton;
        private Button _saveButton;
        private List<Mortar> _lstMortar;

        private const float NB_MORTAL = 5;

        public bool Mode { get => _mode; set => _mode = value; }

        public GameManager(bool mode, string musiqueName = "", string replayName = "")
        {
            // Si mode = true, on est dans le mode libre, si mode = false, on est dans le mode replay
            Mode = mode;
            _lstMortar = new List<Mortar>();
            if (Mode)
            {
                if (musiqueName != "")
                {
                    // Charge la musique
                }
                _saveButton = new Button(new Vector2(0.89f, 0.01f), 0.1f, 0.05f, "Sauvegarder", Color.Gray, Color.White, "save");
            }
            else
            {
                // Charge le replay
            }
            _menuButton = new Button(new Vector2(0.01f, 0.01f), 0.1f, 0.05f, "Accueil", Color.Gray, Color.White, "goBack");

            float height = 0.15f;
            for (int i = 1; i <= NB_MORTAL; i++)
            {
                _lstMortar.Add(new Mortar(new Vector2(Globals.ScreenWidth / (NB_MORTAL + 1) * i / Globals.ScreenWidth , 1 - height), 0.025f, height , Color.White));
            }
        }

        public void Update()
        {
            _menuButton.Update();
            if (Mode)
            {
                Globals.LstComete.ForEach(x => x.Update());
                Globals.LstComete.RemoveAll(c => c.Destroy);
                _saveButton.Update();
                if (InputManager.HasClicked)
                {
                    int nbMortar = Globals.RandomInt(0, (int)NB_MORTAL - 1);
                    Vector2 emitPos = _lstMortar[nbMortar].Position;
                    emitPos.X += _lstMortar[nbMortar].Width /2;
                    Globals.LstComete.Add(new Comete(emitPos, _lstMortar[nbMortar].Angle, 400, 1.5f));
                }
            }
            else
            {
                // Affiche le replay
            }
        }

        public void Draw()
        {
            _menuButton.Draw();
            _lstMortar.ForEach(m => m.Draw());
            if (Mode)
            {
                _saveButton.Draw();
            }
            else
            {
                // Affiche le replay
                // Affiche les données du replay
            }
        }
    }
}
