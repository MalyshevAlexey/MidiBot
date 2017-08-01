using MidiBot.MidiLib;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiBot.NUnitTests
{
    [TestFixture]
    class MidiLibTests
    {
        string inDeviceName = "midiTest";
        string outDeviceName = "midiTest";
        Midi midi;

        [OneTimeSetUp]
        public void Init()
        {
            midi = new Midi();
        }

        [TestCase]
        public void OutOpenTest()
        {
            int result = midi.OutOpen(outDeviceName);
            Assert.IsTrue(result == 0);
            //midi.OutClose();
        }

        [TestCase]
        public void InOpenTest()
        {
            int result = midi.InOpen(inDeviceName);
            Assert.IsTrue(result == 0);
            //midi.InClose();
        }

        [TestCase]
        public void SendMidiTest()
        {
            //midi.OutOpen(outDeviceName);
            int result = midi.SendMidi(new byte[] { 0x80, 0x3C, 0x00, 0x00 });
            Assert.IsTrue(result == 0);
            midi.OutClose();
        }
    }
}
