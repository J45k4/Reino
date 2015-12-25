using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Matrix
{
    public class Cpu : INotifyPropertyChanged
    {

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                On_property_changed("Name");
            }
        }

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
