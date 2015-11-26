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
using System.Threading;

namespace Reino
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpClient client = new TcpClient();
        Thread connectThread;
        public MainWindow()
        {
            InitializeComponent();

            connectThread = new Thread(new ThreadStart(Try_to_connect));
            connectThread.Start();

        }

        private void Try_to_connect()
        {
            while (true) {
                try
                {
                    client.Connect("192.168.180.98", 5555);
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        Enable_buttons();
                    }));
                    break;

                } catch (SocketException e)
                {
                    string message = "Yhdistettäessä tapautui virhe... Yritetäänkö uudelleen?";
                    string caption = "Virheilmoitus";
                    MessageBoxButton buttons = MessageBoxButton.OKCancel;
                    // Show message box
                    MessageBoxResult result = MessageBox.Show(message, caption, buttons);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            continue;
                        case MessageBoxResult.Cancel:
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                this.Shutdown();
                            }));
                            break;
                    }
                }
            }
        }

        private void Enable_buttons()
        {
            TykkiNappula.IsEnabled = true;
            StreamiNappula.IsEnabled = true;
            IPcameraNappula.IsEnabled = true;
            OmenatvNappula.IsEnabled = true;
        }

        private void Disable_buttons()
        {
            TykkiNappula.IsEnabled = false;
            StreamiNappula.IsEnabled = false;
            IPcameraNappula.IsEnabled = false;
            OmenatvNappula.IsEnabled = false;
        }

        public void Shutdown()
        {
            connectThread.Abort();
            client.Close();
            this.Close();
        }

        private void Change_to_tykki(object sender, RoutedEventArgs e)
        {
            Byte[] command = new byte[5] { 0x2, 0x47, 0x82, 0x82, 0x3 };
            client.Client.Send(command);
            command = new byte[5] { 0x2, 0x47, 0x85, 0x82, 0x3 };
            client.Client.Send(command);
            command = new byte[5] { 0x2, 0x47, 0x86, 0x82, 0x3 };
            client.Client.Send(command);
            command = new byte[5] { 0x2, 0x47, 0x87, 0x82, 0x3 };
            client.Client.Send(command);
        }

        private void Change_to_stram(object sender, RoutedEventArgs e)
        {
            Byte[] command = new byte[5] { 0x2, 0x47, 0x82, 0x82, 0x3 };
            client.Client.Send(command);
            command = new byte[5] { 0x2, 0x47, 0x85, 0x82, 0x3 };
            client.Client.Send(command);
            command = new byte[5] { 0x2, 0x47, 0x86, 0x82, 0x3 };
            client.Client.Send(command);
            command = new byte[5] { 0x2, 0x47, 0x87, 0x83, 0x3 };
            client.Client.Send(command);
        }

        private void Change_to_IPcamera(object sender, RoutedEventArgs e)
        {
            Byte[] command = new byte[5] { 0x2, 0x47, 0x82, 0x82, 0x3 };
            client.Client.Send(command);
            command = new byte[5] { 0x2, 0x47, 0x85, 0x82, 0x3 };
            client.Client.Send(command);
            command = new byte[5] { 0x2, 0x47, 0x86, 0x82, 0x3 };
            client.Client.Send(command);
            command = new byte[5] { 0x2, 0x47, 0x87, 0x84, 0x3 };
            client.Client.Send(command);
        }

        private void Change_to_omena(object sender, RoutedEventArgs e)
        {
            Byte[] command = new byte[5] { 0x2, 0x47, 0x82, 0x82, 0x3 };
            client.Client.Send(command);
            command = new byte[5] { 0x2, 0x47, 0x85, 0x82, 0x3 };
            client.Client.Send(command);
            command = new byte[5] { 0x2, 0x47, 0x86, 0x82, 0x3 };
            client.Client.Send(command);
            command = new byte[5] { 0x2, 0x47, 0x87, 0x85, 0x3 };
            client.Client.Send(command);
        }

        private void Close_window(object sender, EventArgs e)
        {
            
        }

        private void Window_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Shutdown();
        }
    }
}
