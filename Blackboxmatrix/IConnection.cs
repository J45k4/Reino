using System;
using System.Collections.Generic;
using System.Text;

namespace Blackboxmatrix
{
    public delegate void ProsessMessageDelegate(byte[] buffer);
    public interface IConnection
    {
        event System.EventHandler NewResponse;
        event System.EventHandler NewComand;

        ProsessMessageDelegate New_message { get; set; }

        void Connect(string ip, int port);
        void Send(Byte[] buffer);
    }
}
