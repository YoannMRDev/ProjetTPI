using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiFiSy_TPI.ParticleCreator;
using NAudio.Midi;

namespace MiFiSy_TPI.GameElement
{
    internal class JamstikMidiListener
    {
        private MidiIn _midi;
        private bool _isConnected;
        private GameManager _gameManager;

        public bool IsConnected { get => _isConnected; set => _isConnected = value; }

        public JamstikMidiListener(GameManager gameManager)
        {
            _gameManager = gameManager;
            IsConnected = false;
            if (MidiIn.NumberOfDevices == 0)
            {
                Debug.Print("Aucun périphérique MIDI d'entrée n'a été trouvé.");
            }
            else
            {
                Debug.Print("Liste des périphériques MIDI d'entrée :");
                for (int i = 0; i < MidiIn.NumberOfDevices; i++)
                {
                    MidiInCapabilities capabilities = MidiIn.DeviceInfo(i);
                    Debug.Print($"ID: {i}, Device ID: {capabilities.ProductId}, Device Name: {capabilities.ProductName}");
                    // Connexion au Jamstik
                    if (capabilities.ProductName == "Jamstik")
                    {
                        _midi = new MidiIn(i);
                        _midi.MessageReceived += MidiIn_MessageReceived;
                        _midi.Start();
                        IsConnected = true;
                        break;
                    }
                }

                if (!IsConnected)
                {
                    Debug.Print("Aucun 'Jamstick' trouvé");
                }
            }
        }

        /// <summary>
        /// Méthode appelé évènements MIDI, traite seulement les évènements de notes, on ou off
        /// </summary>
        private void MidiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
            MidiEvent midiEvent = MidiEvent.FromRawMessage(e.RawMessage);
            if (midiEvent is NoteEvent noteEvent)
            {
                if (noteEvent.CommandCode == MidiCommandCode.NoteOn)
                {
                    // Corde 1 jouée
                    if (noteEvent.Channel - 1 == 1)
                    {
                        _gameManager.CreateComete(noteEvent.Velocity);
                    }
                    // Corde 2 jouée
                    else if (noteEvent.Channel - 1 == 2)
                    {
                        _gameManager.CreateParticleRain(noteEvent.Velocity);
                    }
                }
            }
        }
    }
}
