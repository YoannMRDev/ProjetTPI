using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiFiSy_TPI.ParticleCreator;
using NAudio.Midi;

namespace MiFiSy_TPI.GameElement
{
    internal class JamstikMidiListener
    {
        private MidiIn _midi;
        private bool _isConnected;
        private SpriteFont _font;

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
            if (Globals.ActualPage == Globals.AllPage.Game && Globals.GameManager.Mode)
            {
                MidiEvent midiEvent = MidiEvent.FromRawMessage(e.RawMessage);
                if (midiEvent is NoteEvent noteEvent)
                {
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

        public void DrawErrorNotConnected()
        {
            if (!_isConnected)
            {
                Globals.SpriteBatch.DrawString(_font, "Aucune entree MIDI trouve", new Vector2(Globals.ScreenWidth / 2, Globals.ScreenHeight / 2), Color.Red);
            }
        }
    }
}
