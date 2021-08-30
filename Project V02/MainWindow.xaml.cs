using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
using System.Windows.Threading;
using Microsoft.Win32;
using System.Threading;
using System.ComponentModel;

namespace roject_V01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String newSRC;
        Double CurrrentVolume=0;

        DispatcherTimer Timer ;
        // private static BackgroundWorker backgroundWorker;
        public MainWindow()
        {
            InitializeComponent();
            this.Title = " DJ Remixxx";
            Timer =new DispatcherTimer();
        }

        //close
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Playbtn_Click(object sender, RoutedEventArgs e)
        {
            //ضغطت بلاي فوسط الاغنية 
            if (myMedia.HasAudio || myMedia.HasVideo)
            {
                myMedia.Play();
            }
            else // مفيش ميديا اصلا شغالة ولا اخترت حاجة 
            {
                newSRC = playList.Items[1].ToString();
                playList.SelectedItem = playList.Items[1];

                Manpulate();

            }

        }
        private void Timer_Tick_Tack_Dom(object sender, EventArgs e)
        {
            if (myMedia.HasAudio || myMedia.HasVideo)
            {
                Progress_Play.Value = myMedia.Position.TotalSeconds;
                //change timer 
                try
                {
                    //use remove to delete anything else rather hh:mmm:ss
                    progress_time.Content = String.Format(@"{0:hh\:mm\:ss}",
                    myMedia.Position).Remove(8);
                    
                }
                catch
                {
                    progress_time.Content = String.Format(@"{0:hh\:mm\:ss}",
                    myMedia.Position);
                }

                ////to transform from song to another after first is ended

                if (myMedia.Position == myMedia.NaturalDuration)
                {
                    //if current is not  last  then move to next song //in middle
                    //if last= true
                    //يعني اول حاجة بعد اخر اغنية فالليست ..يعني اول اغنية
                    if (! playList.Items.IsCurrentAfterLast)
                    {
                        int c = playList.SelectedIndex + 1;
                        if (playList.Items.MoveCurrentToPosition(c))
                        {
                            newSRC = playList.Items[c].ToString();
                            playList.SelectedItem = playList.Items[c];

                            Manpulate();
                           
                        }


                        //       
                    }
                    //item in middle // there are items at next//play next song  
                    else
                    {

                       
                        newSRC = playList.Items[1].ToString();
                        playList.SelectedItem = playList.Items[1];

                        Manpulate();
                     

                    }//else in middle
                }

            }
        }


        private void playList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Play();           
        }

        private void Pausebtn_Click(object sender, RoutedEventArgs e)
        {
            myMedia.Pause();
        }

        private void muteButt_Click(object sender, RoutedEventArgs e)
        {
        
            if (myMedia.IsMuted)
            {
               // MessageBox.Show(myMedia.IsMuted.ToString());
                myMedia.Volume = CurrrentVolume ;
                Mute.Source = new BitmapImage(new Uri("images/micro.ico",UriKind.Relative));
                myMedia.IsMuted = false;
            }
            else
            {
                myMedia.Volume = 0;
                Mute.Source = new BitmapImage(new Uri("images/mute.png", UriKind.Relative));
                myMedia.IsMuted = true;
            }
           // Dispatcher d =myMedia.Dispatcher;
            SystemSounds.Hand.Play();
        }

        private void Open_folder_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = true;
            //openFile.ShowDialog();
            if (openFile.ShowDialog()==true)
            {
                //MessageBox.Show("selected");
                foreach (String item in openFile.FileNames)
                {
                    playList.Items.Add(item);
                }
            }
        }

        private void Show_playlist_Click(object sender, RoutedEventArgs e)
        {
            playList.Visibility = Visibility.Visible;
        }

        private void Hide_List_Click(object sender, RoutedEventArgs e)
        {
           var xx= playList.SelectedItem;
            playList.Visibility= Visibility.Collapsed;
        }


        private void Sound_Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (myMedia.IsMuted)
            {
                myMedia.IsMuted = false;
                Mute.Source = new BitmapImage(new Uri("images/micro.ico", UriKind.Relative));
            }
            myMedia.Volume = Sound_Volume.Value;
            CurrrentVolume = myMedia.Volume;
        }
        
        private void backward_btn_Click(object sender, RoutedEventArgs e)
        {
            double r = myMedia.SpeedRatio;
           // MessageBox.Show(r.ToString());
            myMedia.SpeedRatio = r / 1.2;
            speed.Content = myMedia.SpeedRatio.ToString();
        }

        private void forward_btn_Click(object sender, RoutedEventArgs e)
        {
            double r = myMedia.SpeedRatio;
           // MessageBox.Show(r.ToString());
            myMedia.SpeedRatio = r * 1.2;
            speed.Content = myMedia.SpeedRatio.ToString();
            
        }

        private void Play()
        {
            //int hour = 00;
            //int min = 00;

            if (playList.SelectedItem != null)
            {
                newSRC = playList.SelectedItem.ToString();
                Manpulate();
         
            }
            else
            {
                newSRC = playList.Items[1].ToString();
                playList.SelectedItem = playList.Items.IndexOf(newSRC);

                Manpulate();
            }
        }

        private void Manpulate()
        {
            //mp4 /flv /webm  /wmv => video 
            //mp3 / wav / wma /m3u => audio
            //double----------------------------------------------------------------------------------------
           
            if (newSRC.ToUpper().Contains(".MP3") || newSRC.ToUpper().Contains(".WMA") ||
                 newSRC.ToUpper().Contains(".MP4") || newSRC.ToUpper().Contains(".FLV") ||
                 newSRC.ToUpper().Contains(".WMV") || newSRC.ToUpper().Contains(".WEBM") ||
                 newSRC.ToUpper().Contains(".WAV") || newSRC.ToUpper().Contains(".M3U"))
            {
                //  myMedi

                //   //   //change icon depend on file type either audio or video
                // 1)in case audio
                if (newSRC.ToUpper().Contains(".MP3") || newSRC.ToUpper().Contains(".WMA") ||
                     newSRC.ToUpper().Contains(".WAV") || newSRC.ToUpper().Contains(".M3U"))
                {
                    this.Icon = new BitmapImage(new Uri("Music.ico", UriKind.Relative));
                    //bg // myMedia.LayoutTransform = new BitmapImage(new Uri("Casque audio.ico", UriKind.Relative));
                }
                else // 2) in case video
                {
                    this.Icon = new BitmapImage(new Uri("Movie.ico", UriKind.Relative));

                }

                //   //   // Play 
                myMedia.Source = new System.Uri(newSRC);
                playList.SelectedItem = playList.Items.IndexOf(myMedia.Source);
                myMedia.Play();

                //   //   //change title
                string[] arr = myMedia.Source.Segments;

                int last = arr.Length;
                string name_of_song = arr[last - 1];
                this.Title = name_of_song;

                //    // calculate the time of song
                Thread.Sleep(2000);

                if (myMedia.NaturalDuration.HasTimeSpan)
                {
                    
                    double tSec = myMedia.NaturalDuration.TimeSpan.TotalSeconds;
                    Progress_Play.Maximum = tSec;

                    try
                    {
                        song_Duration.Content = "/ " + String.Format(@"{0:hh\:mm\:ss}", myMedia.NaturalDuration).Remove(8); 

                    }
                    catch
                    {
                        song_Duration.Content = "/ " + String.Format(@"{0:hh\:mm\:ss}", myMedia.NaturalDuration);

                    }

                    //increment Each sec
                    Timer.Interval = TimeSpan.FromSeconds(1);
                    Timer.Tick += new EventHandler(Timer_Tick_Tack_Dom);
                    Timer.Start();
                }

            }
        }

        private void Progress_Play_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
                   double width = Progress_Play.ActualWidth;
                   double x =  e.GetPosition(Progress_Play).X;
                   double totaltime = myMedia.NaturalDuration.TimeSpan.TotalSeconds;
                   double result=x/width ;
                    Timer.Stop();

                    Progress_Play.Value = result * totaltime;
                    TimeSpan tt = TimeSpan.FromSeconds(Progress_Play.Value);
                    myMedia.Position = tt;
                    Timer.Start();
        }
    }

    


}
