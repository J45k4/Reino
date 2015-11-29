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
        Thread socketThread;
        Address addr = new Address();
        public bool socketThreadRunning
        {
            get; set;
        }

        public ProsessMessageDelegate New_message { get; set; }

        public Connection(string ip, int port)
        {
            addr.Ip = ip;
            addr.Port = port;
            socketThread = new Thread(new ThreadStart(Worker));
            socketThread.Start();
        }


        private void Worker()
        {
            while (!socketThreadRunning) {
                if (!client.Connected)
                {
                    try
                    {
                        client = new TcpClient(addr.Ip, addr.Port);
                    }
                    catch (SocketException e)
                    {

                    }
                } else
                {
                    NetworkStream stream;
                    try
                    {
                        stream = client.GetStream();
                    
                    byte[] buffer = new byte[512];
                    while(!socketThreadRunning)
                    {
                        try
                        {
                            int bytes = stream.Read(buffer, 0, buffer.Length);
                        } catch (SocketException e) { break; }
                        New_message(buffer);
                    }
                    }
                    catch (SocketException e) { }
                }
            }

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
            if (client.Connected)
            {
                client.Client.Send(buffer);
            }
        }
    }

    struct Address
    {
        public string Ip;
        public int Port;
    }
}
