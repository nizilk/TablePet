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
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TablePet.Services;
using TablePet.Services.Controllers;
using TablePet.Win.Notes;

namespace TablePet.Win.Chat
{
    /// <summary>
    /// ChatInput.xaml 的交互逻辑
    /// </summary>
    public partial class ChatInput : Window
    {
        private ChatService chatService = new ChatService();
        private NoteService noteService;
        public ChatInput()
        {
            InitializeComponent();
            Task chatTask = Task.Run(() =>
            {
                string t = chatService.AskGpt("请你向我打招呼。");
                UpdateTextOut(t);
            });
        }


        public ChatInput(NoteService noteService)
        {
            InitializeComponent();
            Task chatTask = Task.Run(() =>
            {
                string t = chatService.AskGpt("请你向我打招呼。");
                UpdateTextOut(t);
            });
            this.noteService = noteService;
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
                    case "Check Notes":
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            MenuNote noteM = new MenuNote(noteService);
                            noteM.Show();
                        }));
                        break;
                    case "Recommend Music":
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
            rtb_Out.Dispatcher.Invoke(new Action(() =>
            {
                rtb_Out.AppendText(text + "\n");
            }));            
        }

        private void ChatWin_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

    }
}
