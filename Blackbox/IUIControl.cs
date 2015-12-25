using System;
using System.Collections.Generic;
using System.Text;

namespace Matrix
{
    public interface IUIControl
    {
        event System.EventHandler<PortStateChangedEventArgs> PortStateChanged;
        event System.EventHandler<NameChangedEventArgs> NameChanged;

        int Width { get; }
        int Heigth { get; }

        void Video_from_to(int con, int cpu);
        void Kwm_from_to(int cpu, int con);
        void Turn_off_video(int con);
        void Turn_off_kwm(int kwm);
        void Close();
        void Refresh();
    }
}
