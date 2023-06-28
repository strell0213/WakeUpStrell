using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WakeUpStrell_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        MediaPlayer player;
        string filename = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            timer = new System.Windows.Threading.DispatcherTimer();
            player = new MediaPlayer();
        }
        int countsecApp = 0;
        int countsecNow = 0;
        int NowApp = 0;
        int ti = 0;

        private void HoursUp_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(HoursText.Text) >= 23)
            {
                HoursText.Text = "00";
            }
            else
            {
                if (Convert.ToInt32(HoursText.Text) < 9)
                {
                    HoursText.Text = "0" + Convert.ToString(Convert.ToInt32(HoursText.Text) + 1);
                }
                else
                {
                    HoursText.Text = Convert.ToString(Convert.ToInt32(HoursText.Text) + 1);
                }
            }
        }

        private void MinuteUp_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(MinuteText.Text) >= 60)
            {
                MinuteText.Text = "00";
            }
            else
            {
                if (Convert.ToInt32(MinuteText.Text) < 9)
                {
                    MinuteText.Text = "0" + Convert.ToString(Convert.ToInt32(MinuteText.Text) + 1);
                }
                else
                {
                    MinuteText.Text = Convert.ToString(Convert.ToInt32(MinuteText.Text) + 1);
                }
            }
        }

        private void NowButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(HoursText.Text) == 0 && Convert.ToInt32(MinuteText.Text) != 0)
            {
                countsecApp += (24 * 3600) + (Convert.ToInt32(MinuteText.Text) * 60);
                countsecNow += (DateTime.Now.Hour * 3600) + (DateTime.Now.Minute * 60);
            }
            else if (Convert.ToInt32(HoursText.Text) == 0 && Convert.ToInt32(MinuteText.Text) == 0)
            {
                countsecApp += (24 * 3600) + (0 * 60);
                countsecNow += (DateTime.Now.Hour * 3600) + (DateTime.Now.Minute * 60);
            }
            else if (Convert.ToInt32(HoursText.Text) != 0 && Convert.ToInt32(MinuteText.Text) == 0) {
                countsecApp += (Convert.ToInt32(HoursText.Text) * 3600) + (0 * 60);
                countsecNow += (DateTime.Now.Hour * 3600) + (DateTime.Now.Minute * 60);
            }
            else if (Convert.ToInt32(HoursText.Text) != 0 && Convert.ToInt32(MinuteText.Text) != 0) {
                countsecApp += (Convert.ToInt32(HoursText.Text) * 3600) + (Convert.ToInt32(MinuteText.Text) * 60);
                countsecNow += (DateTime.Now.Hour * 3600) + (DateTime.Now.Minute * 60);
            }
            
            if (countsecApp > countsecNow)
            {
                NowApp = countsecApp - countsecNow;
            }
            else if (countsecNow > countsecApp)
            {
                NowApp = (countsecApp + (24*3600)) - countsecNow;
            }

            MessageBox.Show(DateTime.Now.Hour.ToString() + " " + DateTime.Now.Minute.ToString());
            timer.Tick += new EventHandler(timertick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void timertick(object sender, EventArgs e)
        {

            if (ti == NowApp)
            {
                try
                {

                    player.Open(new Uri(filename));
                    player.Play();
                    if (MessageBox.Show("Остановить будильник?", "WakeUpStrell!", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        player.Stop();
                        timer.Stop();
                        NowApp = 0;
                        countsecNow = 0;
                        countsecApp = 0;
                        ti = 0;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                ti++;
            }
        }

        private void HoursDown_click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(HoursText.Text) <= 0)
            {
                HoursText.Text = "23";
            }
            else
            {
                if (Convert.ToInt32(HoursText.Text) < 9)
                {
                    HoursText.Text = "0" + Convert.ToString(Convert.ToInt32(HoursText.Text) - 1);
                }
                else
                {
                    HoursText.Text = Convert.ToString(Convert.ToInt32(HoursText.Text) - 1);
                }
            }
        }

        private void MinuteDown_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(MinuteText.Text) <= 0)
            {
                MinuteText.Text = "23";
            }
            else
            {
                if (Convert.ToInt32(MinuteText.Text) < 9)
                {
                    MinuteText.Text = "0" + Convert.ToString(Convert.ToInt32(MinuteText.Text) - 1);
                }
                else
                {
                    MinuteText.Text = Convert.ToString(Convert.ToInt32(MinuteText.Text) - 1);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (FileGrid.Visibility == Visibility.Hidden)
            {
                FileGrid.Visibility = Visibility.Visible;
            }
            else
            {
                FileGrid.Visibility = Visibility.Hidden;
            }
        }

        private void OpenAudioButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "MP3 файлы (*.mp3)|*.mp3;|Все файлы (*.*)|*.*",
                Multiselect = false,
                ValidateNames = true
            };
            if (ofd.ShowDialog() == true)
            {
                filename = ofd.FileName;
                SourceSoundText.Text = filename;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Plus10HourButton_Click(object sender, RoutedEventArgs e)
        {
            HoursText.Text = Convert.ToString(Convert.ToInt32(HoursText.Text) + 10);
            if (Convert.ToInt32(HoursText.Text) >= 23)
            {
                HoursText.Text = "00";
            }
            else
            {
                if (Convert.ToInt32(HoursText.Text) < 9)
                {
                    HoursText.Text = "0" + HoursText.Text;
                }
                else
                {

                }
            }
        }

        private void Minus10HourButton_Click(object sender, RoutedEventArgs e)
        {
            HoursText.Text = Convert.ToString(Convert.ToInt32(HoursText.Text) - 10);
            if (Convert.ToInt32(HoursText.Text) <= 0)
            {
                HoursText.Text = "23";
            }
            else
            {
                if (Convert.ToInt32(HoursText.Text) < 9)
                {
                    HoursText.Text = "0" + HoursText.Text;
                }
                else
                {

                }
            }
        }

        private void Plus10MinuteButton_Click(object sender, RoutedEventArgs e)
        {
            MinuteText.Text = Convert.ToString(Convert.ToInt32(MinuteText.Text) + 10);
            if (Convert.ToInt32(MinuteText.Text) >= 60)
            {
                MinuteText.Text = "00";
            }
            else
            {
                if (Convert.ToInt32(MinuteText.Text) < 9)
                {
                    MinuteText.Text = "0" + MinuteText.Text;
                }
                else
                {

                }
            }
        }

        private void Minus10MinuteButton_Click(object sender, RoutedEventArgs e)
        {
            MinuteText.Text = Convert.ToString(Convert.ToInt32(MinuteText.Text) - 10);
            if (Convert.ToInt32(MinuteText.Text) <= 0)
            {
                MinuteText.Text = "23";
            }
            else
            {
                if (Convert.ToInt32(MinuteText.Text) < 9)
                {
                    MinuteText.Text = "0" + MinuteText.Text;
                }
                else
                {

                }
            }
        }
    }
}
