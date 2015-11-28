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
using System.Net.Sockets;
using System.Net;
using Blackboxmatrix;

namespace ReinoPro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlackboxMatrix blackbox = new ACX0816(new Connection("192.168.180.98", 5555));
        public MainWindow()
        {
            InitializeComponent();
            
            MainFrame.Navigate(new MatrixControl(blackbox));
        }

        private void komento(object sender, RoutedEventArgs e)
        {
            TcpClient client = new TcpClient("192.168.180.98", 5555);
            Byte[] komento = new byte[5] { 0x2, 0x47, 0x82, 0x82, 0x3 };

            client.Client.Send(komento);
        }
    }
}
