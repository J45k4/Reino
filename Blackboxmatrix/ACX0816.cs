using System;
using System.Collections.Generic;
using System.Text;

namespace Blackboxmatrix
{
    public class ACX0816 : BlackboxMatrix
    {
        public ACX0816(): base()
        {
            Init();
        }

        public ACX0816(IConnection connection): base(connection)
        {
            Init();

        }

        private void Init()
        {
            inputs = new Input[24];
            ios = new Io[8];
            /*for (int i = 0; i < 24; i++)
            {
                inputs[i] = new Input();
            }
            for (int i = 0; i < 24; i++)
            {
                ios[i] = new Io();
            }*/
        }
    }
}
