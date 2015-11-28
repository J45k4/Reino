using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blackboxmatrix;

namespace Blackboxmatrix
{
    public class PortStateChangedEventArgs: EventArgs
    {
        public int Index { get; set; }
        public IoType Io { get; set; }
    }
}