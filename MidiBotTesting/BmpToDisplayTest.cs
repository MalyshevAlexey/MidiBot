using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;

namespace MidiBotTesting
{
    [TestClass]
    public class BmpToDisplayTest
    {
        Bitmap bmp;
        Graphics g;
        Pen pen;
        object locker = new object();
        static ushort[] xOrMasks = { 0xf3e7, 0xffe7 };

        [TestMethod]
        public void TestMethod1()
        {
            bmp = new Bitmap(960, 160, PixelFormat.Format24bppRgb);
            g = Graphics.FromImage(bmp);
            pen = new Pen(Color.Green);
            g.Clear(Color.Coral);
            g.Flush();
            BitmapData bmpdata = bmp.LockBits(new Rectangle(0, 0, 960, 160), ImageLockMode.ReadOnly, bmp.PixelFormat);
            int numbytes = bmpdata.Stride * bmp.Height;
            byte[] bytedata = new byte[numbytes];
            IntPtr ptr = bmpdata.Scan0;
            Marshal.Copy(ptr, bytedata, 0, numbytes);
            bmp.UnlockBits(bmpdata);
            byte[] frame = new byte[327680];
            int count = 0;
            for (int y = 0; y < 160; y++)
            {
                for (int x = 0; x < 960; x++)
                {
                    int next = x + y * 960;
                    int pixel = bytedata[next++] >> 3;
                    pixel <<= 6;
                    pixel += bytedata[next++] >> 2;
                    pixel <<= 5;
                    pixel += bytedata[next++] >> 3;
                    byte[] temp = BitConverter.GetBytes(pixel ^ xOrMasks[x % 2]);
                    frame[count++] = temp[0];
                    frame[count++] = temp[1];
                }
                count += 128;
            }
            //ushort[] frame = new ushort[163840];
            //DateTime start = DateTime.Now;
            //for (int c = 0; c < 10; c++)
            //{
            //    int count = 0;
            //    lock (locker)
            //    {
            //        for (int y = 0; y < 160; y++)
            //        {
            //            for (int x = 0; x < 960; x++)
            //            {
            //                Color color = bmp.GetPixel(x, y);
            //                int pixel = color.B >> 3;
            //                pixel <<= 6;
            //                pixel += color.G >> 2;
            //                pixel <<= 5;
            //                pixel += color.B >> 3;
            //                frame[count++] = (ushort)(pixel ^ xOrMasks[x % 2]);
            //            }
            //            count += 128 / 2;
            //        }
            //    }
            //}
            //Console.WriteLine("New: " + (DateTime.Now - start));
        }

        [TestMethod]
        public void Old()
        {
            bmp = new Bitmap(960, 160);
            g = Graphics.FromImage(bmp);
            pen = new Pen(Color.DarkRed);
            g.Clear(Color.Coral);
            g.Flush();
            byte[] frame = new byte[327680];
            for (int c = 0; c < 10; c++)
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
        }
    }
}
