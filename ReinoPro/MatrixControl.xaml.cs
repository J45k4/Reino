using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using Blackboxmatrix;

namespace ReinoPro
{
    /// <summary>
    /// Interaction logic for MatrixControl.xaml
    /// </summary>
    public partial class MatrixControl : Page
    {
        UniformGrid matrix = new UniformGrid();
        BlackboxMatrix blackbox;
        public MatrixControl(BlackboxMatrix blackbox)
        {
            try { InitializeComponent(); }
            catch (Exception ex)
            {
                MessageBox.Show(Environment.StackTrace);

            }
            this.blackbox = blackbox;
            this.blackbox.Port_state_changed += Blackbox_Port_state_changed;
            matrix.Columns = blackbox.Inputs.Length;
            matrix.Rows = blackbox.Ios.Length;

            for (int i = 0; i < blackbox.Ios.Length; i++)
            {
                for (int j = 0; j < blackbox.Inputs.Length; j++)
                {
                    Button b = new Button();
                    b.Width = 50;
                    b.Height = 50;
                    b.Background = Brushes.White;
                    matrix.Children.Add(b);
                    b.MouseLeftButtonDown += B_MouseLeftButtonDown;
                    b.MouseRightButtonDown += B_MouseRightButtonDown;
                }
            }

            MatrixGrid.Children.Add(matrix);
            Resize_Grid();
        }

        private void Blackbox_Port_state_changed(object sender, PortStateChangedEventArgs e)
        {
            if (e.Io == IoType.KVM)
            {
                Button b = (Button)matrix.Children[e.Index];
                b.Background = Brushes.Green;
            } else if (e.Io == IoType.VideoOnly)
            {
                Button b = (Button)matrix.Children[e.Index];
                b.Background = Brushes.Orange;
            } else if (e.Io == IoType.EMPTY)
            {
                Button b = (Button)matrix.Children[e.Index];
                b.Background = Brushes.White;
            }
        }

        private void B_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Set_button((Button)sender, Brushes.Orange, IoType.VideoOnly);
        }

        private void B_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            Set_button((Button)sender, Brushes.Green, IoType.KVM);
        }

        private void Set_button(Button b, SolidColorBrush color, IoType type)
        {
            if (b.Background == color)
            {
                b.Background = Brushes.White;
                blackbox.Io_off(matrix.Children.IndexOf(b) / matrix.Columns);
            } else
            {
                int y = matrix.Children.IndexOf(b) / matrix.Columns;
                for (int i = 0; i < matrix.Columns; i++)
                {
                    Button Bu = (Button)matrix.Children[y * matrix.Columns + i];
                    if (Bu.Background != Brushes.White)
                    {
                        Bu.Background = Brushes.White;
                        blackbox.Io_off(y);
                    }
                }
                b.Background = color;
                blackbox.Set(matrix.Children.IndexOf(b) % matrix.Columns, y, type);           
            }
        }

        private void Resize_Grid()
        {  
            /*for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {

                }
            }*/

            foreach (Button b in matrix.Children)
            {
                b.Height = this.Height / matrix.Rows;
                b.Width = this.Width / matrix.Columns;
            }
        }

        private void Window_size_changed(object sender, SizeChangedEventArgs e)
        {
            Resize_Grid();
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            blackbox.Refresh();
        }
    }
}
