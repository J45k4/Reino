using System;
using System.Collections.Generic;
using System.Text;

namespace Blackboxmatrix
{
    class Connection : IControl
    {
        private IConnection connection;

        public Connection(IConnection connection)
        {
            this.connection = connection;
            this.connection.New_message = new ProsessMessageDelegate(Prosess_message);
        }

        private void Prosess_message(Byte[] buffer)
        {

        }

        private void Connection_NewResponse(object sender, EventArgs e)
        {
            
        }

        public void Change_state_to(Io state, IoType to)
        {
            
        }

        public void Set_input(Io io, Input input)
        {
            
        }

        public void Io_off(Io Io)
        {
        }

        public void Input_off(Input input)
        {
        }

        public void Connect(string ip, int port)
        {
            
        }

        public void Refresh()
        {
            byte[] packet = new byte[] { 0x2, 0x53, 0x03 };
            connection.Send(packet);
        }

    }
}
