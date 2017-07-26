using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using MidiBot.UsbLib;
using System.Reflection;
using MidiBot.Push2;

namespace MidiBotTesting
{
    [TestClass]
    public class BmpToDisplayTest
    {
        Bitmap bmp;
        Graphics g;
        Pen pen;
        PrivateObject obj;
        byte[] bytedata;
        byte[] frame;
        object locker = new object();
        static ushort[] xOrMasks = { 0xf3e7, 0xffe7 };

        private void Init()
        {
            bmp = new Bitmap(960, 160, PixelFormat.Format24bppRgb);
            g = Graphics.FromImage(bmp);
            pen = new Pen(Color.DarkRed);
            g.Clear(Color.Coral);
            g.Flush();
            Push2Controller push2 = new Push2Controller();
            obj = new PrivateObject(push2);
            bytedata = (byte[])obj.Invoke("BmpToArray", new object[] { bmp, 960, 160 });
            frame = new byte[327680];
        }

        [TestMethod]
        public void PixelCovertTest()
        {
            Init();
            Color pixel = bmp.GetPixel(0, 0);
            int pixel_r = (pixel.R & 0xF8) >> 3;
            int pixel_g = (pixel.G & 0xFC) >> 2;
            int pixel_b = (pixel.B & 0xF8) >> 3;
            int pixel565 = (pixel_r + (pixel_g << 5) + (pixel_b << 11));
            int actualPixel = (int)obj.Invoke("PixelConverter", new object[] { bytedata[0], bytedata[1], bytedata[2] });
            Assert.AreEqual(pixel565, actualPixel);
        }

        [TestMethod]
        public void XoringTest()
        {
            Init();
            int pixel565 = 21503;
            byte upper = (byte)(pixel565 >> 8);
            byte lower = (byte)(pixel565 & 0xFF);
            byte upperXor = (byte)(upper ^ 0xF3);
            byte lowerXor = (byte)(lower ^ 0xE7);
            int pixel565Xor = (pixel565 ^ 0xF3E7);

            Console.WriteLine(Convert.ToString(pixel565, 2).PadLeft(16, '0'));
            Console.WriteLine(Convert.ToString((upper << 8) + lower, 2).PadLeft(16, '0'));
            Console.WriteLine();
            Assert.AreEqual(pixel565, (upper << 8) + lower);

            Console.WriteLine(Convert.ToString(upper, 2).PadLeft(8, '0'));
            Console.WriteLine(Convert.ToString(0xF3, 2).PadLeft(8, '0'));
            Console.WriteLine(Convert.ToString(upperXor, 2).PadLeft(8, '0'));
            Console.WriteLine();

            Console.WriteLine(Convert.ToString(lower, 2).PadLeft(8, '0'));
            Console.WriteLine(Convert.ToString(0xE7, 2).PadLeft(8, '0'));
            Console.WriteLine(Convert.ToString(lowerXor, 2).PadLeft(8, '0'));
            Console.WriteLine();

            Console.WriteLine(Convert.ToString((upperXor << 8) + lowerXor, 2).PadLeft(16, '0'));
            Console.WriteLine(Convert.ToString(pixel565Xor, 2).PadLeft(16, '0'));
            Assert.AreEqual((upperXor << 8) + lowerXor, pixel565Xor);
        }

        [TestMethod]
        public void New()
        {
            Init();
            byte[] newFrame = new byte[327680];
            for (int c = 0; c < 1; c++)
            {
                int count = 0;
                int next = 0;
                lock (locker)
                {
                    for (int y = 0; y < 160; y++)
                    {
                        for (int x = 0; x < 960; x++)
                        {
                            int pixel = bytedata[next++] >> 3;
                            pixel <<= 6;
                            pixel += bytedata[next++] >> 2;
                            pixel <<= 5;
                            pixel += bytedata[next++] >> 3;
                            int pixelXor = pixel ^ xOrMasks[x % 2];
                            newFrame[count++] = (byte)(pixelXor & 0xFF);
                            newFrame[count++] = (byte)(pixelXor >> 8);
                        }
                        count += 128;
                    }
                }
            }
        }

        [TestMethod]
        public void Old()
        {
            Init();
            for (int c = 0; c < 1; c++)
            {
                int count = 0;
                lock (locker)
                {
                    for (int y = 0; y < 160; y++)
                    {
                        byte[] data = new byte[2048];
                        int b = 0;
                        for (int x = 0; x < 960; x++)
                        {
                            Color pixel = bmp.GetPixel(x, y);
                            int pixel_r = (pixel.R & 0xF8) >> 3;
                            int pixel_g = (pixel.G & 0xFC) >> 2;
                            int pixel_b = (pixel.B & 0xF8) >> 3;
                            int pixel565 = pixel_r + (pixel_g << 5) + (pixel_b << 11);
                            data[b++] = (byte)(pixel565 & 255);
                            data[b++] = (byte)(pixel565 >> 8);
                        }
                        for (int i = 0; i < 1920; i += 4)
                        {
                            frame[count++] = data[i + 0] ^= 0xE7;
                            frame[count++] = data[i + 1] ^= 0xF3;
                            frame[count++] = data[i + 2] ^= 0xE7;
                            frame[count++] = data[i + 3] ^= 0xFF;
                        }
                        count += 128;
                    }
                }
            }
            //byte[] actual = (byte[])obj.Invoke("MakeFrame", new object[] { bytedata });
            //Assert.AreEqual(frame, actual);
        }
    }
}
