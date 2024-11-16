using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TablePet.Services.Models;

namespace TablePet.Services.Controllers
{
    public class NoteService
    {
        private readonly NoteContext db;
        public NoteService(NoteContext context)
        {
            db = context;
            Notes.Add(new Note() { NoteId = "0", NoteTitle = "测试机一号", CreatedDate=DateTime.Now, NoteContent="内容内容内容。" });
        }

        // 因为数据库这边还不太会，先用这个
        public ObservableCollection<Note> Notes { get; set; } = new ObservableCollection<Note>();

        public List<Note> NotesB
        {
            get
            {
                return db.Notes
                       .ToList<Note>();
            }
        }


        public void AddNote(Note note) 
        {
            note.NoteId = Guid.NewGuid().ToString();
            note.CreatedDate = DateTime.Now;
            // db.Notes .Add(note);
            // db.SaveChanges();
            Notes.Add(note);
        }

        public void RemoveNote(Note note) => Notes.Remove(note);

        public void UpdateNote(Note note, Note note_org)
        {
            RemoveNote(note_org);
            AddNote(note);
        }
    }
}
