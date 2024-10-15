using Notifications.Wpf;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using XamlAnimatedGif;

namespace TablePat_Win
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public NotificationManager notificationManager = new NotificationManager();
        private DispatcherTimer timer = new DispatcherTimer();

        public Uri[] ResourceOnce = {
            new Uri("pack://application:,,,/Resources/headpicture.gif"),       // 0 headpicture
            new Uri("pack://application:,,,/Resources/profilepicture.gif"),    // 1 profilepicture
            new Uri("pack://application:,,,/Resources/start.gif"),             // 2 start
        };
        public Uri[] Resource = {
            new Uri("pack://application:,,,/Resources/movel.gif"),             // 0 movel
            new Uri("pack://application:,,,/Resources/mover.gif"),             // 1 mover
            new Uri("pack://application:,,,/Resources/relax.gif"),             // 2 relax
            new Uri("pack://application:,,,/Resources/relaxl.gif"),            // 3 relaxl
            new Uri("pack://application:,,,/Resources/interact.gif"),          // 4 interact
            new Uri("pack://application:,,,/Resources/interactl.gif"),         // 5 interactl
            new Uri("pack://application:,,,/Resources/sit.gif"),               // 6 sit
            new Uri("pack://application:,,,/Resources/sitl.gif"),              // 7 sitl
            new Uri("pack://application:,,,/Resources/sleep.gif"),             // 8 sleep
            new Uri("pack://application:,,,/Resources/sleepl.gif"),            // 9 sleepl
        };
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private int threshold = 7;
        private void timer_Tick(object sender, EventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed) return;
            if (Mouse.RightButton == MouseButtonState.Pressed) return;
            int ranTrigger = new Random().Next(0, 50);
            int ranType = new Random().Next(0, 24);
            if (ranTrigger < threshold)
            {
                AnimationBehavior.SetSourceUri(pet, Resource[ranType%10]);
                threshold = threshold > 0 ? threshold - 3 : 0;
                threshold = threshold < 8 ? threshold : 8;
            }
            else
            {
                threshold = threshold < 23 ? threshold + 1 : 23;
            }
        }

        private void mainWin_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                AnimationBehavior.SetSourceUri(pet, Resource[7]);
                DragMove();
            }
        }

        private void mainWin_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AnimationBehavior.AddAnimationCompletedHandler(pet, animationCompleted);
            AnimationBehavior.SetRepeatBehavior(pet, new RepeatBehavior(1));
            AnimationBehavior.SetSourceUri(pet, ResourceOnce[2]);
        }

        public void animationCompleted(object sender, AnimationCompletedEventArgs e)
        {

            AnimationBehavior.SetSourceUri(pet, Resource[2]);
            AnimationBehavior.SetRepeatBehavior(pet, RepeatBehavior.Forever);
            AnimationBehavior.RemoveAnimationCompletedHandler(pet, animationCompleted);
        }

        private void pet_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Random random = new Random();
            int randomNumber = random.Next(1, 7);

            string title = "TablePet";
            string message = "";

            switch (randomNumber)
            {
                case 1:
                    message = "在想我的事？";
                    break;
                case 2:
                    message = "别紧张，我只是来查看你的身体状况。";
                    break;
                case 3:
                    message = "你醒了吗，还是还在梦中？";
                    break;
                case 4:
                    message = "现在你只需要安心制定行动计划。";
                    break;
                case 5:
                    message = "Mon3tr，采集好我需要的组织样本。";
                    break;
                case 6:
                    message = "你似乎更加适应自己的工作和职责了，更像一个领导者了。";
                    break;
            }
            notificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = NotificationType.Information
            });
        }

    }
}
