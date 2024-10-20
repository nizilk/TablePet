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
using System.Windows.Forms;
using XamlAnimatedGif;
using TablePet.Win.Notes;
using TablePet.Services;
using TablePet.Services.Models;

namespace TablePet.Win
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.PrimaryScreen;
        private float screenWidth = 0;
        private float screenHeight = 0;
        private double ratW;
        private double ratH;
        private double curRight;
        private double curDown;
        private NotificationManager notificationManager = new NotificationManager();    // 通知
        private DispatcherTimer timer = new DispatcherTimer();
        private DispatcherTimer timerMove = new DispatcherTimer();
        public NoteContext db = new NoteContext();

        // 全部动画资源的路径 -- 只用一次的
        public Uri[] ResourceOnce = {
            new Uri("pack://application:,,,/Resources/headpicture.gif"),       // 0 headpicture
            new Uri("pack://application:,,,/Resources/profilepicture.gif"),    // 1 profilepicture
            new Uri("pack://application:,,,/Resources/start.gif"),             // 2 start
        };
        //  全部动画资源的路径 -- 随机切换的
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

            screenWidth = screen.Bounds.Width;    // 获取屏幕的宽度     
            screenHeight = screen.Bounds.Height;  // 获取屏幕的高度

            ratW = screenWidth / 1920.0;
            ratH = screenHeight / 1080.0;

            pet.Width *= ratW;
            pet.Height = pet.Width;
            this.Width *= ratW;
            this.Height *= ratH;

            timer.Interval = TimeSpan.FromSeconds(1);
            timerMove.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += timer_Tick;
            timerMove.Tick += timerMove_Tick;
            timer.Start();
            timerMove.Start();
        }


        // 1s时钟tick: 随机改变动画
        private int threshold = 7;  // 初始阈值
        private void timer_Tick(object sender, EventArgs e)     
        {
            if (timer == null)  return;
            if (!changeable)    return;
            if (Mouse.LeftButton == MouseButtonState.Pressed) return;
            if (Mouse.RightButton == MouseButtonState.Pressed) return;

            int ranTrigger = new Random().Next(0, 50);  // 是否更换动画
            int ranType = new Random().Next(0, 54);     // 更换动画编号, move和relax可能性更大些
            int ranAdd = new Random().Next(0, 2);       // 阈值每次有1/2几率+1(使得换动画概率增大)
            if (ranTrigger < threshold)     // 动态缩放阈值, 控制更换动画的频率
            {
                AnimationBehavior.SetSourceUri(pet, Resource[ranType % 10]);
                threshold = threshold > 0 ? threshold - 5 : 0;
                threshold = threshold < 8 ? threshold : 7;
            }
            else
            {
                threshold = threshold < 23 ? threshold + ranAdd : 23;
            }
        }


        // 1ms时钟tick: 移动窗口
        private double step = 0.185;
        private void timerMove_Tick(object sender,  EventArgs e)    
        {
            Uri state = AnimationBehavior.GetSourceUri(pet);
            if (state == null) return;
            if (!moveable) return;

            Point ptLeftUp = this.PointToScreen(new Point(0, 0));   // 左上角屏幕坐标
            Point ptRightDown = this.PointToScreen(new Point(this.ActualWidth, this.ActualHeight));     // 右下角屏幕坐标

            if (state == Resource[0])
            {
                if (ptLeftUp.X < -145*ratW) return;
                this.Left -= step*ratW;
            }
            if (state == Resource[1])
            {
                if (screenWidth - ptRightDown.X < -145*ratW) return;
                mainWin.Left += step*ratW;
            }
        }


        // 鼠标拖动
        private void mainWin_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)    
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                AnimationBehavior.SetSourceUri(pet, Resource[7]);
                DragMove();
            }
        }


        // 松开鼠标左键, 播放动画"start.gif"
        private void mainWin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)     
        {
            AnimationBehavior.AddAnimationCompletedHandler(pet, animationCompleted);
            AnimationBehavior.SetRepeatBehavior(pet, new RepeatBehavior(1));
            AnimationBehavior.SetSourceUri(pet, ResourceOnce[2]);
        }


        // 动画播放完毕时, 回到初始状态"relax.gif"
        public void animationCompleted(object sender, AnimationCompletedEventArgs e)    
        {

            AnimationBehavior.SetSourceUri(pet, Resource[2]);
            AnimationBehavior.SetRepeatBehavior(pet, RepeatBehavior.Forever);
            AnimationBehavior.RemoveAnimationCompletedHandler(pet, animationCompleted);
        }


        // 单击触发随机对话, 通过通知显示, 后续需改进
        private void pet_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)  
        {
            Random random2 = new Random();
            int messageProbability = random2.Next(1, 5);
            if (messageProbability != 1) return;
            
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


        /*==================== 右键选单 ====================*/
        // 置于顶层
        private void pinTop_Click(object sender, RoutedEventArgs e)
        {
            pinTop.IsChecked = !pinTop.IsChecked;
            this.Topmost = pinTop.IsChecked;
        }


        // 调整大小 -- 小
        private async void small_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                small.Dispatcher.Invoke(new Action(delegate { small.IsChecked = true; } ) );
                mid.Dispatcher.Invoke(new Action(delegate { mid.IsChecked = false; } ) );
                large.Dispatcher.Invoke(new Action(delegate { large.IsChecked = false; } ) );
                pet.Dispatcher.Invoke(new Action(delegate
                {
                    pet.Width = 133 * ratW;
                    pet.Height = pet.Width;
                }));
                this.Dispatcher.Invoke(new Action(delegate { this.Width = 165 * ratW; }));
                step = 0.165 * ratW;
            });
            
        }

        // 调整大小 -- 中
        private async void mid_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                small.Dispatcher.Invoke(new Action(delegate { small.IsChecked = false; } ) );
                mid.Dispatcher.Invoke(new Action(delegate { mid.IsChecked = true; } ) );
                large.Dispatcher.Invoke(new Action(delegate { large.IsChecked = false; } ) );
                pet.Dispatcher.Invoke(new Action(delegate
                {
                    pet.Width = 165 * ratW;
                    pet.Height = pet.Width;
                }));
                this.Dispatcher.Invoke(new Action(delegate { this.Width = 165 * ratW; }));
                step = 0.185 * ratW;
            });
        }

        // 调整大小 -- 大
        private async void large_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                small.Dispatcher.Invoke(new Action(delegate { small.IsChecked = false; }));
                mid.Dispatcher.Invoke(new Action(delegate { mid.IsChecked = false; }));
                large.Dispatcher.Invoke(new Action(delegate { large.IsChecked = true; }));
                pet.Dispatcher.Invoke(new Action(delegate
                {
                    pet.Width = 200 * ratW;
                    pet.Height = pet.Width;
                }));
                this.Dispatcher.Invoke(new Action( delegate { this.Width = 200 * ratW; }));
                step = 0.2 * ratW;
            });
        }


        // 禁止走动
        private bool moveable = true;
        private void stopMove_Click(object sender, RoutedEventArgs e)
        {
            stopMove.IsChecked = !stopMove.IsChecked;
            moveable = !stopMove.IsChecked;
        }


        // 坐下
        private void sit_Click(object sender, RoutedEventArgs e) => AnimationBehavior.SetSourceUri(pet, Resource[6]);
        

        // 睡觉
        private bool changeable = true;
        private void sleep_Click(object sender, RoutedEventArgs e)
        {
            if (sleep.Header.ToString() == "睡觉")
            {
                changeable = false;
                AnimationBehavior.SetSourceUri(pet, Resource[8]);
                sleep.Header = "唤醒";
                sit.IsEnabled = false;
            }
            else
            {
                changeable = true;
                AnimationBehavior.SetSourceUri(pet, Resource[2]);
                sleep.Header = "睡觉";
                sit.IsEnabled = true;
            }
        }
        
        
        // 退出
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }


        /*---------- 扩展功能入口 ----------*/
        private void note_new_Click(object sender, RoutedEventArgs e)
        {
            EditNote note = new EditNote(db);
            note.Show();
        }
        /*---------- 扩展功能入口 ----------*/


        /*==================== 右键选单 ====================*/
    }
}
