using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using CodeHollow.FeedReader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TablePet.Services;
using TablePet.Services.Controllers;
using TablePet.Win.Calendar;
using TablePet.Win.FeedReader;
using TablePet.Win.Messagebox;
using TablePet.Win.Notes;
using TablePet.Services.Models;
using TablePet.Win.Alarm;
using Notifications.Wpf;

namespace TablePet.Win.Chat
{
    /// <summary>
    /// ChatInput.xaml 的交互逻辑
    /// </summary>
    public partial class ChatInput : Window
    {
        private MainWindow mainWindow;
        private ChatService chatService;
        private NoteService noteService;
        private FeedReaderService feedReaderService;
        private CalendarService calendarService;

        public ObservableCollection<FeedExt> Feeds;
        public List<string> Folders { get; set; }
        public ChatInput()
        {
            InitializeComponent();
            Task chatTask = Task.Run(() =>
            {
                string t = chatService.AskGpt("请你向我打招呼。");
                UpdateTextOut(t);
            });
        }


        public ChatInput(MainWindow mainWindow, ChatService chatService, NoteService noteService, FeedReaderService feedReaderService, CalendarService calendarService)
        {
            InitializeComponent();
            Task chatTask = Task.Run(() =>
            {
                string t = chatService.AskGpt("请你向我打招呼。");
                UpdateTextOut(t + "\r\n");
            });
            this.mainWindow = mainWindow;
            this.chatService = chatService;
            this.noteService = noteService;
            this.feedReaderService = feedReaderService;
            this.calendarService = calendarService;
        }


        private void bt_In_Click(object sender, RoutedEventArgs e)
        {
            string pm = tb_In.Text;
            tb_In.Clear();
            string ans = "";
            JObject job;
            Task chatTask = Task.Run(() =>
            {
                string intent = chatService.QueryRec(pm);
                job = JObject.Parse(intent);
                switch (job["intention"].ToString())
                {
                    case "Create Note":
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            EditNote noteE = new EditNote(noteService);
                            noteE.Show();
                        }));
                        break;
                    case "All Notes":
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            MenuNote noteM = new MenuNote(noteService);
                            noteM.Show();
                        }));
                        break;
                    case "Feed Reader":
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            FeedView feedView = new FeedView(feedReaderService);
                            feedView.Show();
                        }));
                        break;
                    case "Calendar":
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            CalendarWindow calendar = new CalendarWindow(calendarService);
                            calendar.Show();
                        }));
                        break;
                    case "Alarm":
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            AlarmsWindow alarmsWindow = new AlarmsWindow();
                            alarmsWindow.Show();
                        }));
                        break;
                    case "Web Browser":
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            System.Diagnostics.Process.Start("https://www.baidu.com");
                        }));
                        break;
                    case "End":
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            this.Close();
                        }));
                        break;
                    case "Chat":
                    default:
                        ans = chatService.AskGpt(pm);
                        UpdateTextOut(ans);
                        break;
                }
            });
        }


        private void UpdateTextOut(string text)
        {
            if (BubbleFlag)
            {
                mainWindow.comicMessageBox.Dispatcher.Invoke(new Action(() =>
                {
                    mainWindow.comicMessageBox.ShowMessage(text);
                }));
            }
            else
            {
                rtb_Out.Dispatcher.Invoke(new Action(() =>
                {
                    rtb_Out.AppendText("\r\n" + text + "\r\n");
                }));
            }         
        }


        private void ChatWin_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }


        private bool BubbleFlag = false;
        private void bt_chatSwitch_Click(object sender, RoutedEventArgs e)
        {
            BubbleFlag = !BubbleFlag;
            rtb_Out.Visibility = BubbleFlag ? Visibility.Hidden : Visibility.Visible;
            pt_chatSwitch.Fill = BubbleFlag ? Brushes.White : Brushes.Gray;
            pt_chatClose.Fill = BubbleFlag ? Brushes.White : Brushes.Gray;
        }


        private void bt_chatClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
