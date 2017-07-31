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
        string inDeviceName = "testMidi";
        string outDeviceName = "testMidi";

        [TestMethod]
        public void GetInIdByNameTest()
        {
            PrivateObject obj = new PrivateObject(new Midi());
            int id = (int)obj.Invoke("GetInIdByName", new object[] { inDeviceName });
            Assert.IsTrue(id > -1);
        }

        [TestMethod]
        public void GetInIdByNameExceptionTest()
        {
            PrivateObject obj = new PrivateObject(new Midi());
            var ex = Assert.ThrowsException<Exception>(() => obj.Invoke("GetInIdByName", new object[] { "noname" }));
            
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

        //[TestMethod]
        //public void ReceiveTest()
        //{
        //    Midi midi = new Midi();
        //    int result = midi.InOpen(inDeviceName);
        //    result = midi.OutOpen(outDeviceName);
        //    result = midi.SendSysex(new byte[] { 0xF0, 0x00, 0x21, 0x1D, 0x01, 0x01, 0x0A, 0x00, 0xF7 });
        //    Thread.Sleep(10);
        //    Assert.IsTrue(midi.longAnswer.Data.Length > 0 );
        //    midi.InClose();
        //    midi.OutClose();
        //}
    }
}
