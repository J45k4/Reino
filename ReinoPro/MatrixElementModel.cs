using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Blackboxmatrix;

namespace ReinoPro
{
    class MatrixElementModel
    {
        Io io;

        public MatrixElementModel(Io io)
        {
            this.io = io;
        }

        public SolidColorBrush Color
        {
            get
            {
                if (io is KVM)
                {
                    return Brushes.Green;
                } else if (io is VideoOnly)
                {
                    return Brushes.Orange;
                } else
                {
                    return Brushes.White;
                }     
            }
        }
    }
}
