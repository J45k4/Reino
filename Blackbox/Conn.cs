using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Matrix
{
    [Serializable]
    public class Conn : INotifyPropertyChanged
    {

        private string name;

        public string Name
        {
            get { return name; }
            set {
                name = value;
                On_property_changed("Name");
            }
        }

        [field: NonSerialized]
        private int cpuvideo = -1;
        [field: NonSerialized]
        private int cpukwm = -1;
        [field: NonSerialized]
        private ConMode mode = ConMode.OFF;
        public int Cpuvideo {
            get
            {
                return cpuvideo;
            }
            set
            {
                cpuvideo = value;
            }
        }


        public int Cpukwm
        {
            get { return cpukwm; }
            set { cpukwm = value; }
        }


        public ConMode Mode {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
            }
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
