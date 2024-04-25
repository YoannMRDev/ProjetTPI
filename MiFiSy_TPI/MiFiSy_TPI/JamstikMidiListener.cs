using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiFiSy_TPI.ParticleCreator;
using NAudio.Midi;

namespace MiFiSy_TPI
{
    internal class JamstikMidiListener
    {
        private MidiIn _midi;
        public MidiIn Midi { get => _midi; set => _midi = value; }

        public JamstikMidiListener()
        {
            bool isConnected = false;
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
                        Midi = new MidiIn(i);
                        Midi.MessageReceived += MidiIn_MessageReceived;
                        Midi.Start();
                        isConnected = true;
                        break;
                    }
                }

                if (!isConnected)
                {
                    Debug.Print("Aucun Jamstick trouvé");
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

                    }
                    // Corde 2 jouée
                    else if (noteEvent.Channel - 1 == 2)
                    {

                    }
                }
            }
        }
    }
}
