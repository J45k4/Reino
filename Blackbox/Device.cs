using System;
using System.Collections.Generic;
using System.Text;

namespace Matrix
{
    public class Device : Matrix
    {
        public Device(IConnection conn) : base(conn)
        {

        }

        public override void Kwm_from_to(int con, int cpu)
        {
            throw new NotImplementedException();
        }

        public override void Turn_off_kwm(int kwm)
        {
            throw new NotImplementedException();
        }

        public override void Turn_off_video(int con)
        {
            throw new NotImplementedException();
        }

        public override void Video_from_to(int con, int cpu)
        {
        }
    }
}
