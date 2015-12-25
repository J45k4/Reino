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
using Matrix;

namespace Reinoo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Matrix.Matrix matrix = new ACX0816(new Connection("192.168.180.98", 5555));
            MainFrame.Navigate(new MatrixControl(matrix));
        }

        private void Window_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
    }
}
