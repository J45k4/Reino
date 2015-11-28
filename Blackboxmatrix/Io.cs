using System;
using System.Collections.Generic;
using System.Text;

namespace Blackboxmatrix
{
    public class Io
    {
        private int input;
        private IoType iotype = IoType.EMPTY;

        public int Input
        {
            get
            {
                return input;
            }

            set
            {
                input = value;
            }
        }

        public IoType Iotype
        {
            get
            {
                return iotype;
            }

            set
            {
                iotype = value;
            }
        }
    }

    public enum IoType
    {
        KVM, VideoOnly, EMPTY
    }
}
