using System;
using System.Collections.Generic;
using System.Text;

namespace Matrix
{
    public delegate void ProsessMessageDelegate(byte[] buffer);
    public interface IConnection
    {

        event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
        ProsessMessageDelegate New_message { get; set; }
        bool Listen { get; set; }
        void Send(byte[] message);

        void Close();
    }
}
