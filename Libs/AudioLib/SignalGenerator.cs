using System;

namespace MidiBot.AudioLib
{
    public class SignalGenerator
    {
        private WaveFormat WaveFormat { get; set; }

        public SignalGenerator(WaveFormat waveFormat)
        {
            WaveFormat = waveFormat;
        }
        public double[] GenerateSignal(double frq)
        {
            int _samples = 4096;
            double _frequency = frq;
            double _amplitude = 32768.0;
            double[] values = new double[_samples];
            double theta = 2.0 * Math.PI * _frequency / WaveFormat.SampleRate;
            for (int i = 0; i < _samples; i++)
            {
                values[i] = _amplitude * Math.Sin(i * theta);
            }
            return values;
        }
    }

    
}