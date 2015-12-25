using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace Matrix
{
    [Serializable]
    public abstract class Matrix : IUIControl, INotifyPropertyChanged
    {
        protected Conn[] cons;
        protected Cpu[] cpus;
        [field: NonSerialized]
        protected Dictionary<CommandType, Byte> commands = new Dictionary<CommandType, byte>();
        protected IConnection connection;



        public int Width
        {
            get
            {
                return cpus.Length;
            }
        }

        public int Heigth
        {
            get
            {
                return cons.Length;
            }
        }

        public event EventHandler<PortStateChangedEventArgs> PortStateChanged;
        public event EventHandler<NameChangedEventArgs> NameChanged;

        public Matrix(IConnection conn)
        {
            connection = conn;
            conn.New_message = new ProsessMessageDelegate(Prosess_message);
            init();
        }

        private void init()
        {
            connection.New_message = new ProsessMessageDelegate(Prosess_message);
        }

        abstract public void Video_from_to(int con, int cpu);
        abstract public void Kwm_from_to(int cpu, int con);
        abstract public void Turn_off_video(int con);
        abstract public void Turn_off_kwm(int kwm);

        private void Prosess_message(Byte[] buffer)
        {
            if (buffer[1] == 83)
            {
                Thread.Sleep(500);
                for (int i = 0; i < cons.Length; i++)
                {
                    int x = buffer[i + 2] - 129;
                    //if (cons[i] == null) cons[i] = new Conn();
                    if (x > -1)
                    {
                        cons[i].Cpuvideo = x;
                        Raise_port_changed_event(i, x, ConMode.VIDEO);
                    }
                }
                for (int i = 0; i < cpus.Length; i++)
                {
                    int y = buffer[i + 2 + 16] - 129;
                    if (y > -1)
                    {
                        cons[i].Cpukwm = y;
                        Raise_port_changed_event(y, i, ConMode.KWM);
                    }
                }
            }
        }

        private void Raise_port_changed_event(int i, ConMode mode)
        {
            
            if (PortStateChanged != null)
            {
                Console.WriteLine(mode);
                PortStateChanged(this, new PortStateChangedEventArgs() { Index = i, Mode = mode });
            }
        }

        private void Raise_port_changed_event(int con, int cpu, ConMode mode)
        {
            Raise_port_changed_event(con*cpus.Length+cpu, mode);
        }


        protected Byte Convert_to_byte(int i)
        {
            i++;
            Byte k = 128;
            k += (byte)i;
            return k;
        }

        public void Close()
        {
            connection.Close();
        }

        public void Refresh()
        {
            byte[] packet = new byte[] { 0x2, 0x53, 0x03 };
            connection.Send(packet);
            connection.Listen = true;
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        private void On_property_changed(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
