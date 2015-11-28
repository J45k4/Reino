using System;
using System.Collections.Generic;
using System.Text;

namespace Blackboxmatrix
{
    public abstract class BlackboxMatrix : IControl
    {
        protected Input[] inputs;
        protected Io[] ios;
        private Connection connection;

        public event System.EventHandler<PortStateChangedEventArgs> Port_state_changed;

        public BlackboxMatrix(IConnection conn)
        {
            connection = new Connection(conn);
        }

        public BlackboxMatrix()
        {
            
        }

        public Io[] Ios
        {
            get
            {
                return ios;
            }
        }

        public Input[] Inputs
        {
            get
            {
                return inputs;
            }
        }


        public void Set(int input, int io, IoType type)
        {
            if (ios[io] == null) ios[io] = new Io();
            ios[io].Input = input;
            Change_state_to(io, type);
            /*if (ios[io].Input != input)
            {
                connection.Io_off(ios[io]);
                if (type == IoType.KVM)
                {
                    for (int i = 0; i < ios.Length; i++)
                    {
                        if (ios[i].Input == input)
                        {
                            if (ios[i].Iotype == IoType.KVM)
                            {
                                Change_state_to(ios[i], IoType.VideoOnly);
                                break;
                            }
                        }
                    }
                }
            }*/
        }

        public void Change_state_to(Io state, IoType to)
        {
            connection.Change_state_to(state, to);
            if (Port_state_changed != null)
            {
                Port_state_changed(this, new PortStateChangedEventArgs() { Index = Get_index_of(state), Io = to });
                state.Iotype = to;
            }
        }

        public void Change_state_to(int state, IoType to)
        {
            /*if (Port_state_changed != null)
            {
                Port_state_changed(this, new PortStateChangedEventArgs() { Index = Get_index_of(state), Io = to });
            }*/
        }

        public void Set_input(Io io, Input input)
        {
            connection.Set_input(io, input);
            if (Port_state_changed != null)
            {
                Port_state_changed(this, new PortStateChangedEventArgs() { Index = Get_index_of(io), Io = IoType.EMPTY });
                io.Input = Array.IndexOf(inputs, input);
                Port_state_changed(this, new PortStateChangedEventArgs() { Index = Get_index_of(io), Io = io.Iotype });
                io.Input = Array.IndexOf(inputs, input);
            }
        }

        public void Io_off(Io Io)
        {
            connection.Io_off(Io);
            if (Port_state_changed != null)
            {
                Port_state_changed(this, new PortStateChangedEventArgs() { Index = Get_index_of(Io), Io = IoType.EMPTY });
            }
        }

        public void Io_off(int index)
        {
            if (ios[index] == null) ios[index] = new Io();
            ios[index].Input = -1;
            ios[index].Iotype = IoType.EMPTY;
        }

        public void Input_off(Input input)
        {
            connection.Input_off(input);
        }

        public void Input_off(int index)
        {

        }

        private int Get_index_of(Io io)
        {
            return Array.IndexOf(ios, io) * inputs.Length + io.Input;
        }

        private int Get_index_of(int io)
        {
            return io * inputs.Length + ios[io].Input;
        }

        public void Connect(string ip, int port)
        {
            //connection = new Connection(ip, port);
        }

        public void Refresh()
        {

        }

        public void fill()
        {
            for (int i = 0; i < 24; i++)
            {
                inputs[i] = new Input();
            }
            for (int i = 0; i < 8; i++)
            {
                ios[i] = new Io();
            }
        }
    }
}
