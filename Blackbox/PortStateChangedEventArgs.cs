using System;
using System.Collections.Generic;
using System.Text;

namespace Matrix
{
    public class PortStateChangedEventArgs: EventArgs
    {
        public int Index;
        public ConMode Mode;
    }
}
