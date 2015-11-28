using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackboxmatrix;
using System.Net.Sockets;
using System.Threading;

namespace ReinoPro
{
    class Connection : IConnection
    {
        public event EventHandler NewComand;
        public event EventHandler NewResponse;
        TcpClient client = new TcpClient();
        Thread

        public ProsessMessageDelegate New_message { get; set; }

        public Connection(string ip, int port)
        {
            client.Connect(ip, port);
        }

        public void Connect(string ip, int port)
        {
            throw new NotImplementedException();
        }

        public void Send()
        {
            throw new NotImplementedException();
        }

        public void Send(byte[] buffer)
        {
            throw new NotImplementedException();
        }
    }
}
