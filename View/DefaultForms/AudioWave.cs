using MidiBot.AudioLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidiBot.Views.DefaultForms
{
    public partial class AudioWave : Form
    {
        Bitmap bmpWave;
        Bitmap bmpFFT;
        Graphics g;
        Pen pen;
        WaveIn wi;
        WaveOut wo;
        double[] waveLeft;
        double[] waveRight;
        double[] fftLeft;

        public AudioWave()
        {
            InitializeComponent();
            bmpWave = new Bitmap(pictureWave.Width, pictureWave.Height);
            g = Graphics.FromImage(bmpWave);
            pen = new System.Drawing.Pen(Color.WhiteSmoke);
            wi = new WaveIn();
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(DataAvailable);
            wo = new WaveOut(wi);
            wi.StartRecording();
            wo.StartPlayback();
        }

        void DataAvailable(object sender, WaveInEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            waveLeft = new double[e.Buffer.Length / 2];
            for (int i = 0; i < waveLeft.Length; i++)
                waveLeft[i] = BitConverter.ToInt16(e.Buffer, i * 2);
            //if (trackBar.InvokeRequired) trackBar.Invoke ((MethodInvoker) delegate { waveLeft = wi.signalGenerator.GenerateSignal(trackBar.Value); label.Text = trackBar.Value.ToString(); });

            fftLeft = FFT.FFTDb(ref waveLeft);
            
            RenderFrequencyDomain();
            RenderTimeDomain();
            sw.Stop();
            //Console.WriteLine(sw.ElapsedTicks + " " + sw.Elapsed.TotalMilliseconds + " ");
        }

        public void RenderTimeDomain()
        {
            g.Clear(pictureWave.BackColor);
            pen.Color = Color.WhiteSmoke;
            // Determine channnel boundries
            int width = bmpWave.Width;
            int center = bmpWave.Height / 2;
            int height = bmpWave.Height;

            g.DrawLine(pen, 0, center, width, center);

            int leftLeft = 0;
            int leftTop = 0;
            int leftRight = width;
            int leftBottom = center - 1;

            int rightLeft = 0;
            int rightTop = center + 1;
            int rightRight = width;
            int rightBottom = height;

            // Draw left channel
            double yCenterLeft = (leftBottom - leftTop) / 2;
            double yScaleLeft = 0.5 * (leftBottom - leftTop) / 32768;  // a 16 bit sample has values from -32768 to 32767
            int xPrevLeft = 0, yPrevLeft = 0;
            pen.Color = Color.LimeGreen;
            int koeff = waveLeft.Length / (leftRight - leftLeft);
            for (int xAxis = leftLeft; xAxis < leftRight; xAxis++)
            {
                int yAxis = (int)(yCenterLeft + (waveLeft[koeff * xAxis] * yScaleLeft));
                if (xAxis > 0 ) g.DrawLine(pen, xPrevLeft, yPrevLeft, xAxis, yAxis);
                xPrevLeft = xAxis;
                yPrevLeft = yAxis;
            }
            pictureWave.Image = bmpWave;
        }

        public void RenderFrequencyDomain()
        {
            // Set up for drawing
            bmpFFT = new Bitmap(pictureFFT.Width, pictureFFT.Height);
            Graphics offScreenDC = Graphics.FromImage(bmpFFT);
            SolidBrush brush = new System.Drawing.SolidBrush(Color.FromArgb(0, 0, 0));
            Pen pen = new System.Drawing.Pen(Color.WhiteSmoke);

            // Determine channnel boundries
            int width = bmpFFT.Width;
            int center = bmpFFT.Height / 2;
            int height = bmpFFT.Height;

            offScreenDC.DrawLine(pen, 0, center, width, center);

            int leftLeft = 0;
            int leftTop = 0;
            int leftRight = width;
            int leftBottom = center - 1;

            int rightLeft = 0;
            int rightTop = center + 1;
            int rightRight = width;
            int rightBottom = height;

            // Draw left channel
            for (int xAxis = leftLeft; xAxis < leftRight; xAxis++)
            {
                double amplitude = (int)fftLeft[(int)(((double)(fftLeft.Length) / (double)(width)) * xAxis)];
                if (amplitude < 0) // Drop negative values
                    amplitude = 0;
                int yAxis = (int)(leftBottom - ((leftBottom - leftTop) * amplitude) / 100);  // Arbitrary factor
                pen.Color = Color.FromArgb(0, 0, (int)amplitude % 255);
                offScreenDC.DrawLine(pen, xAxis, leftBottom, xAxis, yAxis);
            }

            // Clean up
            pictureFFT.Image = bmpFFT;
            offScreenDC.Dispose();
        }
    }
}
