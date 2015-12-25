
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Reinoo
{
    public class Connection : IConnection
    {
        public event EventHandler NewComand;
        public event EventHandler NewResponse;
        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
        TcpClient client = new TcpClient();
        Thread listeningThread;
        Thread timerThread;
        Address addr = new Address();

        NetworkStream stream;
        bool listening = false;

        bool receiverRunning = false;
        bool connectorRunning = false;
        bool restartTimer = false;
        bool timerRunning = false;

        int timerTime = 0;
        int exTime = 500;

        public bool socketThreadRunning
        {
            get; set;
        }

        public ProsessMessageDelegate New_message { get; set; }

        public Connection(string ip, int port)
        {
            addr.Ip = ip;
            addr.Port = port;
            Connect();
            timerThread = new Thread(new ThreadStart(Timer));
            timerThread.Start();
            //socketThread = new Thread(new ThreadStart(Worker));
            //timerThread = new Thread(new ThreadStart(Timer));
            //receiverThread = new Thread(new ThreadStart(Receiver));
            //socketThread.Start();
            //timerThread.Start();
        }

        ~Connection()
        {
            Close();
        }


        public bool Listen {
            get
            {
                return listening;
            }
            set
            {
                listening = value;
                if (value)
                {
                    listeningThread = new Thread(new ThreadStart(Listener));
                    listeningThread.Start();
                } else
                {
                    stream.Close();
                    listeningThread.IsBackground = true;
                }
            }
        }

        private void Worker()
        {
            while (!socketThreadRunning) {
                if (!client.Connected)
                {
                    
                } else
                {
                    NetworkStream stream;
                    try
                    {
                        stream = client.GetStream();
                        
                    
                    byte[] buffer = new byte[512];
                    while(!socketThreadRunning)
                    {
                        int bytes = stream.Read(buffer, 0, buffer.Length);
                        New_message(buffer);
                    }
                    }
                    catch (SocketException e) { }
                }
            }

        }

        private void Listener()
        {
            try
            {
                stream = client.GetStream();
                byte[] buffer = new byte[512];
                int bytes = 0;
                while (bytes == 0)
                {
                    bytes = stream.Read(buffer, 0, buffer.Length);
                }
                New_message(buffer);
                stream.Close();
            }  catch (SocketException)
            {

            }
        }

        private void Timer()
        {
            for (; timerTime < exTime; timerTime++)
            {
                Thread.Sleep(10);
            }
            Close();
        }

        private void Reset_timer()
        {
            if (!timerThread.IsAlive)
            {
                timerThread = new Thread(new ThreadStart(Timer));
                timerThread.Start();
            }
            timerTime = 0;
        }


        public void Connect(string ip, int port)
        {
            addr.Ip = ip;
            addr.Port = port;
            Connect();
        }

        public void Connect()
        {
            try
            {
                client = new TcpClient(addr.Ip, addr.Port);
                Console.WriteLine("Yhteyden muodostus onnistui");
                Raise_ConnectionStateChanged_event(ConnectionState.CONNECTED);
                restartTimer = true;
            }
            catch (SocketException e)
            {
                Console.WriteLine("Yhteyden muodostus epäonnistui");
                Raise_ConnectionStateChanged_event(ConnectionState.DISCONNECTED);
            }
        }

        public void Send(byte[] buffer)
        {
            if (client.Connected)
            {
                try {
                    client.Client.Send(buffer);
                    Reset_timer();
                } catch (SocketException)
                {
                    System.Windows.MessageBox.Show("aouaa");
                }

            }  else
            {
                Connect();
                Send(buffer);
            }
        }

        public void Close()
        {
            receiverRunning = false;
            connectorRunning = false;
            timerRunning = false;
            Raise_ConnectionStateChanged_event(ConnectionState.DISCONNECTED);
            Console.WriteLine("Yhteys suljettu");
            if (client.Connected)
            {
                client.GetStream().Close();
                client.Close();
            }
        }

        private void Raise_ConnectionStateChanged_event(ConnectionState state)
        {
            if (ConnectionStateChanged != null)
            {
                ConnectionStateChanged(this, new ConnectionStateChangedEventArgs() { State = state });
            }
        }
    }

    struct Address
    {
        public string Ip;
        public int Port;
    }
}
