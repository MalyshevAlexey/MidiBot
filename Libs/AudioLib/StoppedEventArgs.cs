using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiBot.AudioLib
{
    public class StoppedEventArgs : EventArgs
    {
        private readonly Exception exception;
        public StoppedEventArgs(Exception exception = null)
        {
            this.exception = exception;
        }
        public Exception Exception { get { return exception; } }
    }
}
