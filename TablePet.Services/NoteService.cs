using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TablePet.Services.Models;

namespace TablePet.Services
{
    internal class NoteService
    {
        public NoteService() { }

        public void AddNote(string title, string content) 
        {
            Note note = new Note();
            note.NoteId = Guid.NewGuid().ToString();
            note.NoteTitle = title;
            note.NoteContent = content;
            // 这里需要存数据库
        }
    }
}
