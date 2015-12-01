using System;
using System.Collections.Generic;
using System.Text;

namespace Blackboxmatrix
{
    public abstract class BlackboxMatrix : IControl
    {
        protected Input[] inputs;
        protected Io[] ios;
        private IConnection connection;

        public event System.EventHandler<PortStateChangedEventArgs> Port_state_changed;

        public BlackboxMatrix(IConnection conn)
        {
            connection = conn;
            connection.New_message = new ProsessMessageDelegate(Prosess_message);
        }

        public BlackboxMatrix()
        {
            
        }

        ~BlackboxMatrix()
        {
            connection.Close();
        }

        public void Close()
        {
            connection.Close();
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

        private void Prosess_message(Byte[] buffer)
        {
            if (buffer[1] == 83)
            {
                for (int i = 0; i < Ios.Length; i++)
                {
                    if (ios[i] == null) ios[i] = new Io();
                    ios[i].Input = buffer[i + 2] - 129;
                    if (buffer[i + 2 + 16] - 129 == ios[i].Input)
                    {
                        ios[i].Iotype = IoType.KVM;
                        Raise_port_changed_event(i, buffer[i + 2] - 129, IoType.KVM);
                    } else
                    {
                        ios[i].Iotype = IoType.VideoOnly;
                        Raise_port_changed_event(i, buffer[i + 2] - 129, IoType.VideoOnly);
                    }                    
                }
            }
        }

        public void Set(int i, IoType type)
        {
            Set(i / inputs.Length, i % inputs.Length, type);
        }

        public void Set(int io, int input, IoType type)
        {
            if (type == IoType.EMPTY)
            {
                ios[io].Iotype = IoType.EMPTY;
                ios[io].Input = -1;
            }
            else
            {
                Set_state(io, input);
                if (ios[io] == null)
                {
                    ios[io] = new Io();
                    ios[io].Input = input;
                    ios[io].Iotype = type;                    
                }
                else
                {
                    ios[io].Iotype = type;
                    if (ios[io].Input > -1 && ios[io].Input != input) Raise_port_changed_event(io, ios[io].Input, IoType.EMPTY);
                    ios[io].Input = input;
                }
                if (type == IoType.KVM)
                {
                    for (int i = 0; i < ios.Length; i++)
                    {
                        if (i != io)
                        {
                            if (ios[i] == null) ios[i] = new Io();
                            else
                            {
                                if (ios[i].Input == input)
                                {
                                    if (ios[i].Iotype == IoType.KVM)
                                    {
                                        ios[i].Iotype = IoType.VideoOnly;
                                        Raise_port_changed_event(i, ios[i].Input, IoType.VideoOnly);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void Set_state(int io, int input)
        {
            byte[] packet = new byte[5] { 0x2, 0x47, Convert_to_byte(io), Convert_to_byte(input), 0x3 };
            connection.Send(packet);
        }


        /*
        public void Set(int input, int io, IoType type)
        {
            if (ios[io] == null) ios[io] = new Io();
            ios[io].Input = input;
            Change_state_to(io, type);
            Byte[] command = new byte[5] { 0x2, 0x47, Convert_to_byte(io), Convert_to_byte(input), 0x3 };
            connection.Send(command);

            if (ios[io].Input != input)
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
            }
        }*/


        private void Raise_port_changed_event(int io, int input, IoType type)
        {
            if (Port_state_changed != null)
            {
                Console.WriteLine(io * inputs.Length + input);
                Port_state_changed(this, new PortStateChangedEventArgs() { Index = io * inputs.Length + input, Io = type });
            }
        }


        private Byte Convert_to_byte(int i)
        {
            i++;
            Byte k = 128;
            k += (byte)i;
            return k;
        }

        public void Change_state_to(Io state, IoType to)
        {
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
            if (Port_state_changed != null)
            {
                Port_state_changed(this, new PortStateChangedEventArgs() { Index = Get_index_of(io), Io = IoType.EMPTY });
                io.Input = Array.IndexOf(inputs, input);
                Port_state_changed(this, new PortStateChangedEventArgs() { Index = Get_index_of(io), Io = io.Iotype });
                io.Input = Array.IndexOf(inputs, input);
            }
        }

        public void Set_input(int io, int input)
        {
            if (Port_state_changed != null)
            {
                if (ios[io] == null) ios[io] = new Io();
                Port_state_changed(this, new PortStateChangedEventArgs() { Index = Get_index_of(io), Io = IoType.EMPTY });
                ios[io].Input = input;
                Port_state_changed(this, new PortStateChangedEventArgs() { Index = Get_index_of(io), Io = IoType.VideoOnly });
            }

        }

        public void Io_off(Io Io)
        {
            if (Port_state_changed != null)
            {
                Port_state_changed(this, new PortStateChangedEventArgs() { Index = Get_index_of(Io), Io = IoType.EMPTY });
            }
        }

        public void Io_off(int index)
        {
            byte[] packet = new byte[] { 0x2, 0x48, Convert_to_byte(index), 0x03 };
        }

        public void Input_off(Input input)
        {
        }

        public void Input_off(int index)
        {
            byte[] packet = new byte[] { 0x2, 0x48, Convert_to_byte(index), 0x03 };
            connection.Send(packet);
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
            byte[] packet = new byte[] { 0x2, 0x53, 0x03 };
            connection.Send(packet);
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
