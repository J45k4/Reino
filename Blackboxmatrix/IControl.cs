using System;
using System.Collections.Generic;
using System.Text;

namespace Blackboxmatrix
{
    interface IControl
    {
        void Change_state_to(Io state, IoType to);
        void Input_off(Input input);
        void Io_off(Io io);
        void Set_input(Io io, Input input);
        void Connect(string ip, int port);
        void Refresh();
    }
}
