using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NAudio.Midi;
/*
 * Auteur : Yoann Meier
 * Date : 06/05/2024
 * Projet : Projet TPI, application de simulation de feux d'artifices en 2D
 * Description de la page : Classe pour gérer les entrés MIDI
 */
namespace MiFiSy_TPI.Manager
{
    internal class JamstikMidiListener
    {
        private MidiIn _midi;
        private bool _isConnected;
        private SpriteFont _font;

        /// <summary>
        /// Constructeur de la classe, connection à la guitare
        /// </summary>
        /// <param name="font">font pour le message d'erreur si la guitare n'est pas trouvé</param>
        public JamstikMidiListener(SpriteFont font)
        {
            _font = font;
            _isConnected = false;

            if (MidiIn.NumberOfDevices != 0)
            {
                for (int i = 0; i < MidiIn.NumberOfDevices; i++)
                {
                    MidiInCapabilities capabilities = MidiIn.DeviceInfo(i);
                    // Connexion au Jamstik
                    if (capabilities.ProductName == "Jamstik")
                    {
                        _midi = new MidiIn(i);
                        _midi.MessageReceived += MidiIn_MessageReceived;
                        _midi.Start();
                        _isConnected = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Méthode appelé évènements MIDI, traite seulement les évènements de notes, on ou off
        /// </summary>
        private void MidiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
            // Si on est dans le jeu en mode libre
            if (Globals.ActualPage == Globals.AllPage.Game && Globals.GameManager.Mode)
            {
                MidiEvent midiEvent = MidiEvent.FromRawMessage(e.RawMessage);
                if (midiEvent is NoteEvent noteEvent)
                {
                    // Lorsqu'une note est jouée
                    if (noteEvent.CommandCode == MidiCommandCode.NoteOn)
                    {
                        // Corde 1 jouée
                        if (noteEvent.Channel - 1 == 1)
                        {
                            Globals.GameManager.CreateComete(noteEvent.Velocity);
                        }
                        // Corde 2 jouée
                        else if (noteEvent.Channel - 1 == 2)
                        {
                            Globals.GameManager.CreateParticleRain(noteEvent.Velocity);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Affiche un message d'erreur si il n'y a pas de connexion
        /// </summary>
        public void DrawErrorNotConnected()
        {
            if (!_isConnected)
            {
                Globals.SpriteBatch.DrawString(_font, "Aucune entree MIDI trouve", new Vector2(Globals.ScreenWidth / 2, Globals.ScreenHeight / 2), Color.Red);
            }
        }
    }
}
