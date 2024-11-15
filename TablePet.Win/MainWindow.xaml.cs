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
using TablePet.Win.Chat;
using TablePet.Win.Notes;
using TablePet.Win.Calendar;
using TablePet.Services;
using TablePet.Services.Models;
using TablePet.Services.Controllers;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.Windows.Interop;
using TablePet.Win.FeedReader;
using CodeHollow.FeedReader;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using TablePet.Win.SharingDatan;
using TablePet.Win.ProgressBar;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using System.IO;
using TablePet.Win.Alarm;


namespace TablePet.Win
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private double screenWidth = SystemParameters.PrimaryScreenWidth;
        private double screenHeight = SystemParameters.PrimaryScreenHeight;
        private double DpiRatio;
        
        private NotifyIcon notifyIcon;

        private double ratW;
        private double ratH;
        private NotificationManager notificationManager = new NotificationManager();    // 通知
        private DispatcherTimer timer = new DispatcherTimer();
        private DispatcherTimer timerMove = new DispatcherTimer();
        private DispatcherTimer timerinfo = new DispatcherTimer();

        public NoteContext db = new NoteContext();
        public NoteService noteService;
        private ChatService chatService = new ChatService();
        private CalendarService calendarService = new CalendarService();
        FeedReaderService feedReaderService = new FeedReaderService();
        private CalendarWindow calendarWindow;
        private AlarmsWindow alarmsWindow;

        public ObservableCollection<FeedExt> Feeds { get; set; } = new ObservableCollection<FeedExt>();
        public List<string> Folders { get; set; } = new List<string>() { "Root" };

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
            this.ShowInTaskbar = false;
            
            string iconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\Resources", "logo.ico");
            notifyIcon = new NotifyIcon
            {
                
                Icon = new Icon(iconPath),
                Visible = true,
                Text = "TablePet"
            };

            ratW = screenWidth / 1920.0;
            ratH = screenHeight / 1080.0;

            DpiRatio = Graphics.FromHwnd(new WindowInteropHelper(Application.Current.MainWindow).Handle).DpiX / 96;

            pet.Width *= ratW;
            pet.Height = pet.Width;
            this.Width *= ratW;
            this.Height *= ratH;

            edge = -280 * ratW;

            timer.Interval = TimeSpan.FromSeconds(1);
            timerMove.Interval = TimeSpan.FromMilliseconds(1);
            timerinfo.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += timer_Tick;
            timerMove.Tick += timerMove_Tick;  
            timerinfo.Tick += timerinfo_Tick;
            timer.Start();
            timerMove.Start();
            timerinfo.Start();
            
            noteService = new NoteService(db);
            WelcomeMessage();
            FeelingBar.Progress = SharingData.Favorability;
        }
        
        private void ShowNotificationHandler(string title, string message, NotificationType type)
        {
            showNotification(title, message, type);
        }
        

        private void WelcomeMessage()
        {
            if (SharingData.IsFavorabilityLow()) return;
            System.DateTime currentTime=new System.DateTime(); 
            currentTime = System.DateTime.Now;
            
            if (currentTime.Hour >= 0 && currentTime.Hour < 6)
            {
                showNotification("TablePet", "夜深了，注意休息哦~");
            }
            else if (currentTime.Hour >= 6 && currentTime.Hour < 12)
            {
                showNotification("TablePet", "早上好，今天也要加油哦~");
            }
            else if (currentTime.Hour >= 12 && currentTime.Hour < 18)
            {
                showNotification("TablePet", "下午好，要来杯咖啡休息一下么~");
            }
            else
            {
                showNotification("TablePet", "晚上好，今天也辛苦了~");
            }
        }
        
        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            UpdateFeelingBar();
            FeelingBar.Visibility = Visibility.Visible;
        }
        
        private void UpdateFeelingBar()
        {
            FeelingBar.Dispatcher.Invoke(() => 
            {
                if (FeelingBar is FeelingBar progressBar)
                {
                    progressBar.UpdateProgress(); 
                }
            });
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            FeelingBar.Visibility = Visibility.Hidden;
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
        private double step = 0.335;
        private double edge;
        private void timerMove_Tick(object sender,  EventArgs e)    
        {
            Uri state = AnimationBehavior.GetSourceUri(pet);
            if (state == null) return;
            if (!moveable) return;

            System.Windows.Point ptLeftUp = this.PointToScreen(new System.Windows.Point(0, 0));   // 左上角屏幕坐标
            System.Windows.Point ptRightDown = this.PointToScreen(new System.Windows.Point(this.ActualWidth, this.ActualHeight));     // 右下角屏幕坐标

            if (state == Resource[0])
            {
                if (ptLeftUp.X < edge) return;
                this.Left -= step*ratW;
            }
            if (state == Resource[1])
            {
                if (screenWidth * DpiRatio - ptRightDown.X < edge) return;
                this.Left += step*ratW;
            }
        }


        private void timerinfo_Tick(object sender, EventArgs e)
        {
            Task infoTask = Task.Run(() =>
            {
                var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                // 创建内存占用字节数的性能计数器
                var ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                
                float cpu = cpuCounter.NextValue();
                Thread.Sleep(1000);
                cpu = cpuCounter.NextValue();
                float ram = ramCounter.NextValue();
                
                showNotification("性能使用提示", $"CPU: {cpu}%\nRAM: {ram}MB", NotificationType.Warning);
            });
        }


        // 按下鼠标左键
        private System.Windows.Point lmAbs = new System.Windows.Point();
        private void mainWin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.lmAbs = e.GetPosition(this);
            this.lmAbs.Y = Convert.ToInt16(this.Top) + this.lmAbs.Y;
            this.lmAbs.X = Convert.ToInt16(this.Left) + this.lmAbs.X;
        }


        // 鼠标拖动
        private void mainWin_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                AnimationBehavior.SetSourceUri(pet, Resource[7]);
                System.Windows.Point MousePosition = e.GetPosition(this);
                System.Windows.Point MousePositionAbs = new System.Windows.Point();
                MousePositionAbs.X = Convert.ToInt16(this.Left) + MousePosition.X;
                MousePositionAbs.Y = Convert.ToInt16(this.Top) + MousePosition.Y;
                if(MousePositionAbs.X - this.lmAbs.X > 0)
                    AnimationBehavior.SetSourceUri(pet, Resource[6]);
                this.Left = this.Left + (MousePositionAbs.X - this.lmAbs.X);
                
                this.Top = this.Top + (MousePositionAbs.Y - this.lmAbs.Y);
                this.lmAbs = MousePositionAbs;
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
            Random random = new Random();
            int messageProbability = random.Next(1, 5);
            if (messageProbability != 1) return;

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
            
            showNotification(title, message);

        }
        

        // 气泡显示 or 右下角通知
        private void showNotification(string title, string message, NotificationType type = NotificationType.Information)
        {
            if (type == NotificationType.Information)
            {
                comicMessageBox.ShowMessage(message);
            }
            else notificationManager.Show(new NotificationContent            
            {
                Title = title,
                Message = message,
                Type = type
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
                    pet.Width = 266 * ratW;
                    pet.Height = pet.Width;
                }));
                this.Dispatcher.Invoke(new Action(delegate 
                { 
                    this.Width = 266 * ratW;
                    this.Height = this.Width;
                }));
                step = 0.32 * ratW;
                edge = -220 * ratW;
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
                    pet.Width = 330 * ratW;
                    pet.Height = pet.Width;
                }));
                this.Dispatcher.Invoke(new Action(delegate 
                { 
                    this.Width = 330 * ratW;
                    this.Height = this.Width;
                }));
                step = 0.335 * ratW;
                edge = -280 * ratW;
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
                    pet.Width = 400 * ratW;
                    pet.Height = pet.Width;
                }));
                this.Dispatcher.Invoke(new Action( delegate 
                { 
                    this.Width = 400 * ratW; 
                    this.Height = this.Width;
                }));
                step = 0.45 * ratW;
                edge = -350 * ratW;
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
        // 对话
        private void chatIn_Click(object sender, RoutedEventArgs e)
        {
            ChatInput chatInput = new ChatInput(this, chatService, noteService, Feeds, Folders);
            chatInput.Show();
        }


        // 新建便签
        private void note_new_Click(object sender, RoutedEventArgs e)
        {
            EditNote note = new EditNote(noteService);
            note.Show();
        }


        // 所有便签
        private void note_all_Click(object sender, RoutedEventArgs e)
        {
            MenuNote note = new MenuNote(noteService);
            note.Show();
        }


        // Feed Reader
        private void mi_feed_Click(object sender, RoutedEventArgs e)
        {
            FeedView feedView = new FeedView(Feeds, Folders);
            feedView.Show();
        }


        // 日历
        private void calendar_Click(object sender, RoutedEventArgs e)
        {
            calendarWindow = new CalendarWindow(calendarService);
            calendarWindow.OnNotificationRequested += ShowNotificationHandler;

            calendarWindow.Show();
        }
        
        //闹钟
        private void alarm_Click(object sender, RoutedEventArgs e)
        {
            alarmsWindow = new AlarmsWindow();
            alarmsWindow.Show();
        }


        // 测试功能时用，后续需删除
        private void test_Click(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show(Directory.GetCurrentDirectory());   // D:\Documents\GitHub\TablePet\TablePet.Win\bin\Debug
            // MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory);     // D:\Documents\GitHub\TablePet\TablePet.Win\bin\Debug\
            // MessageBox.Show(Environment.CurrentDirectory);      // D:\Documents\GitHub\TablePet\TablePet.Win\bin\Debug

            // FeedReaderService feedReaderService = new FeedReaderService();
            // Feed feed = feedReaderService.ReadFeed();
            // string origDoc = feed.OriginalDocument;
            // string pattern = @"<dc:creator><!\[CDATA\[([^\]]+)\]\]></dc:creator>";
            // string creater = Regex.Match(origDoc, pattern).Groups[1].Value;
            // MessageBox.Show(creater);
            // FeedItem smp = feed.Items[0];
            // MessageBox.Show(smp.Content);

            //var xml = XElement.Parse("<root>"+smp.Content+"</root>");
            //MessageBox.Show(xml.Value);
        }

        /*---------- 扩展功能入口 ----------*/


        /*==================== 右键选单 ====================*/
    }
}
