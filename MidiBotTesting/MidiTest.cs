using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MidiBot.MidiLib;
using System.Threading;
using System.Runtime.InteropServices;

namespace MidiBotTesting
{
    [TestClass]
    public class MidiTest
    {
        string inDeviceName = "midiTest";
        string outDeviceName = "midiTest";

        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void GetInIdByNameTest()
        {
            PrivateObject obj = new PrivateObject(new Midi());
            int id = (int)obj.Invoke("GetInIdByName", new object[] { inDeviceName });
            Assert.IsTrue(id > -1);
        }

        [TestMethod]
        public void InOpenTest()
        {
            Midi midi = new Midi();
            int result = midi.InOpen(inDeviceName);
            Assert.IsTrue(result == 0);
            midi.InClose();
        }

        [TestMethod]
        public void GetOutIdByNameTest()
        {
            PrivateObject obj = new PrivateObject(new Midi());
            int id = (int)obj.Invoke("GetOutIdByName", new object[] { outDeviceName });
            Assert.IsTrue(id > -1);
        }

        [TestMethod]
        public void OutOpenTest()
        {
            Midi midi = new Midi();
            int result = midi.OutOpen(outDeviceName);
            Assert.IsTrue(result == 0);
            midi.OutClose();
        }

        [TestMethod]
        public void SendMidiTest()
        {
            Midi midi = new Midi();
            midi.OutOpen(outDeviceName);
            int result = midi.SendMidi(new byte[] { 0x80, 0x3C, 0x00, 0x00 });
            Assert.IsTrue(result == 0);
            midi.OutClose();
        }

        [TestMethod]
        public void SendSysexTest()
        {
            Midi midi = new Midi();
            midi.OutOpen(outDeviceName);
            int result = midi.SendSysex(new byte[] { 0xF0, 0x00, 0x21, 0x1D, 0x01, 0x01, 0x0A, 0x00, 0xF7 });
            Assert.IsTrue(result == 0);
            midi.OutClose();
        }

        [TestMethod]
        public void ShortReceiveTest()
        {
            Midi midi = new Midi();
            midi.InOpen(inDeviceName);
            midi.OutOpen(outDeviceName);
            midi.SendMidi(new byte[] { 0x80, 0x3C, 0x00, 0x00 });
            Thread.Sleep(1);
            Assert.IsTrue(midi.ShortAnswer.Data.Length == 4);
            midi.Close();
        }

        [TestMethod]
        public void LongReceiveTest()
        {
            Midi midi = new Midi();
            midi.InOpen(inDeviceName);
            midi.OutOpen(outDeviceName);
            midi.SendSysex(new byte[] { 0xF0, 0x00, 0x21, 0x1D, 0x01, 0x01, 0x0A, 0x00, 0xF7 });
            Thread.Sleep(1);
            Assert.IsTrue(midi.LongAnswer.Data.Length == 9);
            midi.Close();
        }

        [TestMethod]
        public void ShortMultipleReceiveTest()
        {
            Midi midi = new Midi();
            midi.InOpen(inDeviceName);
            midi.OutOpen(outDeviceName);
            midi.SendMidi(new byte[] { 0x90, 0x3C, 0x7F, 0x00 });
            Thread.Sleep(1);
            Assert.IsTrue(midi.ShortAnswer.Data[2] == 0x7F);
            midi.SendMidi(new byte[] { 0x90, 0x3C, 0x6F, 0x00 });
            Thread.Sleep(1);
            Assert.IsTrue(midi.ShortAnswer.Data[2] == 0x6F);
            midi.SendMidi(new byte[] { 0x90, 0x3C, 0x5F, 0x00 });
            Thread.Sleep(1);
            Assert.IsTrue(midi.ShortAnswer.Data[2] == 0x5F);
            midi.SendMidi(new byte[] { 0x90, 0x3C, 0x4F, 0x00 });
            Thread.Sleep(1);
            Assert.IsTrue(midi.ShortAnswer.Data[2] == 0x4F);
            midi.Close();
        }

        [TestMethod]
        public void LongMultipleReceiveTest()
        {
            Midi midi = new Midi();
            midi.InOpen(inDeviceName);
            midi.OutOpen(outDeviceName);
            midi.SendSysex(new byte[] { 0xF0, 0x00, 0x21, 0x1D, 0x01, 0x01, 0x0A, 0x00, 0xF7 });
            Thread.Sleep(1);
            Assert.IsTrue(midi.LongAnswer.Data[6] == 0x0A);
            midi.SendSysex(new byte[] { 0xF0, 0x00, 0x21, 0x1D, 0x01, 0x01, 0x0B, 0x00, 0xF7 });
            Thread.Sleep(1);
            Assert.IsTrue(midi.LongAnswer.Data[6] == 0x0B);
            midi.SendSysex(new byte[] { 0xF0, 0x00, 0x21, 0x1D, 0x01, 0x01, 0x0C, 0x00, 0xF7 });
            Thread.Sleep(1);
            Assert.IsTrue(midi.LongAnswer.Data[6] == 0x0C);
            midi.SendSysex(new byte[] { 0xF0, 0x00, 0x21, 0x1D, 0x01, 0x01, 0x0D, 0x00, 0xF7 });
            Thread.Sleep(1);
            Assert.IsTrue(midi.LongAnswer.Data[6] == 0x0D);
            midi.Close();
        }
    }
}
