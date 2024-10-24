﻿using System;
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
using TablePet.Services;

namespace TablePet.Win
{
    /// <summary>
    /// ChatInput.xaml 的交互逻辑
    /// </summary>
    public partial class ChatInput : Window
    {
        private ChatService chatService = new ChatService();
        public ChatInput()
        {
            InitializeComponent();
            Task chatTask = Task.Run(() =>
            {
                string t = chatService.AskGpt("请你向我打招呼。");
                UpdateTextOut(t);
            });
        }

        private void bt_In_Click(object sender, RoutedEventArgs e)
        {
            string pm = tb_In.Text;
            Task chatTask = Task.Run(() =>
            {
                string t = chatService.AskGpt(pm);
                UpdateTextOut(t);
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
