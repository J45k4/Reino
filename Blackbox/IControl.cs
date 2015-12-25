using System;
using System.Collections.Generic;
using System.Text;

namespace Matrix
{
    interface IControl
    {
        void Set(int con, int cpu, CommandType type);
        void Unset_con(int con);
        void Unset_cpu(int cpu);
    }
}
