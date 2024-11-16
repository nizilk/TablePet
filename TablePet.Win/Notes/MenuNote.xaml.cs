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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TablePet.Services;
using TablePet.Services.Models;
using TablePet.Services.Controllers;

namespace TablePet.Win.Notes
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MenuNote : Window
    {
        private NoteService noteService;
        public MenuNote()
        {
            InitializeComponent();
        }

        public MenuNote(NoteService sv)
        {
            InitializeComponent();
            noteService = sv;
            lb_Notes.ItemsSource = noteService.Notes;
            this.DataContext = this;
        }

        private void bt_folderPlus_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bt_noteNew_Click(object sender, RoutedEventArgs e)
        {
            EditNote note = new EditNote(noteService);
            note.Show();
        }

        private void lb_Notes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var obj = (Note)lb_Notes.SelectedItem;
            if (obj != null)
            {
                EditNote editNote = new EditNote(noteService, obj);
                editNote.Show();
            }
        }

        private void bt_noteClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
