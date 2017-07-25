using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace MidiBotTesting
{
    [TestClass]
    public class BmpToDisplayTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            byte r, g, b;
            r = 1;
            g = 10;
            b = 10;
            int pixel = b >> 3;
            pixel <<= 6;
            pixel += g >> 2;
            pixel <<= 5;
            pixel += r >> 3;
            Trace.WriteLine(Convert.ToString(pixel, 2));
        }
    }
}
