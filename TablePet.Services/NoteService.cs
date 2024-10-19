using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TablePet.Services.Models;

namespace TablePet.Services
{
    public class NoteService
    {
        private readonly NoteContext db;
        public NoteService(NoteContext context)
        {
            db = context;
        }

        public List<Note> Notes
        {
            get
            {
                return db.Notes
                       .ToList<Note>();
            }
        }

        // 因为数据库这边还不太会，先用这个
        public List<Note> NotesTestList { get; } = new List<Note>();

        public void AddNote(Note note) 
        {
            note.NoteId = Guid.NewGuid().ToString();
            note.CreatedDate = DateTime.Now;
            db.Notes .Add(note);
            db.SaveChanges();
            NotesTestList.Add(note);
        }
    }
}
