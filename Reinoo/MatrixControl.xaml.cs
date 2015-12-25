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
    /// Interaction logic for MatrixControl.xaml
    /// </summary>
    public partial class MatrixControl : Page
    {
        IUIControl control;
        Dictionary<ConMode, SolidColorBrush> ColorCodes = new Dictionary<ConMode, SolidColorBrush>();

        Button[,] buttons;
        Button[] lastButtons;
        Button[] kwms;
        int[] kwmIndexes;

        bool kwmvideo = false;
        bool kwm = false;
        bool video = true;
    
        public MatrixControl(IUIControl control)
        {
            InitializeComponent();
            this.control = control;
            Generate_grid();
            init();
            control.PortStateChanged += Control_PortStateChanged;
        }

        private void init()
        {
            ColorCodes.Add(ConMode.KWM, Brushes.Red);
            ColorCodes.Add(ConMode.VIDEO, Brushes.Orange);
            ColorCodes.Add(ConMode.VIDEOKWM, Brushes.Green);
            ColorCodes.Add(ConMode.OFF, Brushes.White);
        }

        private void Control_PortStateChanged(object sender, PortStateChangedEventArgs e)
        {
            Console.WriteLine("Port state changed " + e.Mode);
            this.Dispatcher.Invoke((Action)(() =>
            {
                int y = e.Index / control.Width;
                Button b = (Button)MatrixGrid.Children[e.Index];
                if ((e.Mode != ConMode.OFF && e.Mode != ConMode.VIDEOKWM) && (b.Background == ColorCodes[ConMode.KWM] || b.Background == ColorCodes[ConMode.VIDEO]))
                {
                    b.Background = ColorCodes[ConMode.VIDEOKWM];
                } else b.Background = ColorCodes[e.Mode];
                if (e.Mode == ConMode.KWM)
                {
                    kwms[y] = b;
                } else if (e.Mode == ConMode.VIDEO)
                {
                    lastButtons[y] = b;
                }

            }));
        }

        private void Generate_grid()
        {
            MatrixGrid.Columns = control.Width;
            MatrixGrid.Rows = control.Heigth;
            this.MinWidth = 500;
            this.MinHeight = 400;

            buttons = new Button[control.Heigth, control.Width];
            lastButtons = new Button[control.Width];
            kwms = new Button[control.Heigth];
            kwmIndexes = new int[control.Heigth];

            for (int i = 1; i < control.Width+1; i++) ColumnNumbers.Children.Add(new TextBlock() { Text = i.ToString(), FontSize = 40 });
            for (int i = 1; i < control.Heigth + 1; i++) RowNumbers.Children.Add(new TextBlock() { Text = i.ToString(), FontSize = 40 });
            RowNames.Children.Add(new TextBlock() { Text = "Tykitys pc näyttö 1" });
            for (int i = 0; i < control.Width; i++)
            {
                for (int j = 0; j < control.Heigth; j++)
                {
                    Button b = new Button();
                    b.Background = Brushes.White;
                    b.MouseDown += B_MouseDown;
                    b.Click += B_Click;
                    MatrixGrid.Children.Add(b);
                    //buttons[i, j] = b;
                }
            }
            VideoEnabledButton.IsEnabled = video;
            KwmEnabledButton.IsEnabled = kwm;
            VideoKWMEnabledButton.IsEnabled = kwmvideo;
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            if (kwmvideo)
            {
                Button b = (Button)sender;
                int y = MatrixGrid.Children.IndexOf(b) / control.Width;
                int x = MatrixGrid.Children.IndexOf(b) % control.Width;
                if (b.Background == ColorCodes[ConMode.VIDEOKWM])
                {
                    control.Turn_off_kwm(x);
                    control.Turn_off_video(y);

                    Console.WriteLine("Vaihdetaan ruutu y:" + y + " x:" + x + " tyhjäksi");
                    b.Background = ColorCodes[ConMode.OFF];
                    if (kwmIndexes[y] == x) kwmIndexes[y] = -1;
                }
                else
                {
                    Console.WriteLine("Vaihdetaan ruutu y:" + y + " x:" + x + " videokwm");
                    b.Background = ColorCodes[ConMode.VIDEOKWM];
                    Clean_kwms(x, y, b);
                }
                //control.Set(MatrixGrid.Children.IndexOf(b), ConMode.VIDEOKWM);
                //Clear();
            }
        }

        private void B_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                Button b = (Button)sender;
                if (e.ChangedButton == MouseButton.Middle)
                {
                    if (kwm)
                    {
                        b.Background = Brushes.Red;
                        //control.Set(MatrixGrid.Children.IndexOf(b), ConMode.KWM);
                        //Clear();
                    }
                } else if (e.ChangedButton == MouseButton.Right)
                {
                    if (video)
                    {
                        int y = MatrixGrid.Children.IndexOf(b) / control.Width;
                        int x = MatrixGrid.Children.IndexOf(b) % control.Width;
                        if (b.Background == ColorCodes[ConMode.VIDEO])
                        {
                            b.Background = ColorCodes[ConMode.OFF];
                            lastButtons[y] = null;
                            control.Turn_off_video(y);
                        } 
                        else
                        {
                            if (b.Background == ColorCodes[ConMode.KWM])
                            {
                                control.Video_from_to(y, x);
                                b.Background = ColorCodes[ConMode.VIDEOKWM];
                            }
                            else {
                                b.Background = ColorCodes[ConMode.VIDEO];
                            }
                            control.Video_from_to(y, x);
                            if (lastButtons[y] != null)
                            {
                                if (lastButtons[y].Background != ColorCodes[ConMode.VIDEOKWM]) lastButtons[y].Background = ColorCodes[ConMode.OFF];
                                else
                                {
                                    lastButtons[y].Background = ColorCodes[ConMode.KWM];
                                }

                            }
                            lastButtons[y] = b;
                        }
                    }
                }
            }
        }

        private void Turn_videoControl_on()
        {
            VideoEnabledButton.Background = ColorCodes[ConMode.VIDEO];
        }
        private void Turn_videoControl_off()
        {
            VideoEnabledButton.Background = ColorCodes[ConMode.OFF];
        }

        private void Turn_kwmControl_on()
        {
            KwmEnabledButton.Background = ColorCodes[ConMode.KWM];
        }
        private void Turn_kwmControl_off()
        {
            KwmEnabledButton.Background = ColorCodes[ConMode.OFF];
        }

        private void Turn_videokwmControl_on()
        {
            VideoKWMEnabledButton.Background = ColorCodes[ConMode.VIDEOKWM];
        }
        private void Turn_videokwmControl_off()
        {
            VideoKWMEnabledButton.Background = ColorCodes[ConMode.OFF];
        }

        private void Clean_kwms(int x, int y, Button b)
        {
            int y2 = MatrixGrid.Children.IndexOf(lastButtons[y]) / control.Width;
            int x2 = MatrixGrid.Children.IndexOf(lastButtons[y]) % control.Width;
            if (lastButtons[y] != null)
            {

                if (x != x2)
                {
                    Console.WriteLine("DUBLICATE: Vaihdetaan ruutu y:" + y2 + " x:" + x2 + " tyhjäksi");
                    lastButtons[y].Background = ColorCodes[ConMode.OFF];
                }

            }
            if (x != x2 || y != y2)
            {
            for (int i = 0; i < control.Heigth; i++)
                if (kwmIndexes[i] == x)
                    if (kwms[i] != null)
                    {
                        if (x != kwmIndexes[i] && y != i)
                        {
                            Console.WriteLine("DUBLICATE2: Vaihdetaan ruutu y:" + i + " x: " + kwmIndexes[i] + " tyhjäksi");
                            kwms[i].Background = ColorCodes[ConMode.OFF];
                        }

                    }
                kwms[y] = b;
                kwmIndexes[y] = x;
                lastButtons[y] = b;
            }
        }

        private void Clear()
        {
            foreach(Button b in MatrixGrid.Children)
            {
                b.Background = ColorCodes[ConMode.OFF];
            }
        }

        private void Resize_ui()
        {
            for (int i = 0; i < control.Width; i++)
            {
                TextBlock t = (TextBlock)ColumnNumbers.Children[i];
                t.FontSize = (this.ActualWidth-200) / control.Width;
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Resize_ui();
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            Clear();
            control.Refresh();
        }

        private void Toggle_videokwm_control(object sender, RoutedEventArgs e)
        {
            if (!kwmvideo) {
                Turn_videokwmControl_on();
                kwmvideo = true;
            }else
            {
                Turn_videokwmControl_off();
                kwmvideo = false;
            }
        }

        private void Toggle_kwm_control(object sender, RoutedEventArgs e)
        {
            if (!kwm)
            {
                Turn_kwmControl_on();
                kwm = true;
            }
            else
            {
                Turn_kwmControl_off();
                kwm = false;
            }
        }

        /*private void Clear()
        {
            foreach (Button b in MatrixGrid.Children)
            {
                b.Background = ColorCodes[ConMode.OFF];
            }
        }*/

        private void Toggle_video_control(object sender, RoutedEventArgs e)
        {
            if (!video)
            {
                Turn_videoControl_on();
                video = true;
            }
            else
            {
                Turn_videoControl_off();
                video = false;
            }
        }

        private void Refresh_matrix(object sender, RoutedEventArgs e)
        {
            Clear();
            control.Refresh();
            
        }

        public void Close()
        {
            control.Close();
        }
    }
}
