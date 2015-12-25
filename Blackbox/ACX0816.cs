using System;
using System.Collections.Generic;
using System.Text;

namespace Matrix
{
    public class ACX0816 : Matrix
    {
        public ACX0816(IConnection conn) : base(conn)
        {
            cpus = new Cpu[24];
            cons = new Conn[8];
            for (int i = 0; i < 8; i++) cons[i] = new Conn();
            for (int i = 0; i < 24; i++) cpus[i] = new Cpu();
            commands.Add(CommandType.VIDEO, 0x47);
            commands.Add(CommandType.VIDEOOFF, 0x48);
            commands.Add(CommandType.VIDEOS, 0x49);
            commands.Add(CommandType.ALLVIDEOOFF, 0x4A);
        }

        public override void Kwm_from_to(int con, int cpu)
        {
            byte[] packet = new byte[] { 0x2, 0x4B, Convert_to_byte(cpu), Convert_to_byte(con), 0x3 };
            connection.Send(packet);
        }

        public override void Turn_off_kwm(int kwm)
        {
            
        }

        public override void Turn_off_video(int con)
        {
            
        }

        public override void Video_from_to(int con, int cpu)
        {
            byte[] packet; //= new byte[] { 0x2, 0x48, Convert_to_byte(con), 0x3 };
            //connection.Send(packet);
            packet = new byte[] { 0x2, 0x47, Convert_to_byte(con), Convert_to_byte(cpu), 0x3 };
            Console.WriteLine(packet.ToString());
            connection.Send(packet);
        }
    }
}
