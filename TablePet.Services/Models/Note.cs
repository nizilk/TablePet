using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TablePet.Services.Models
{
    public class Note
    {
        public string NoteId { get; set; } = "";
        public DateTime? CreatedDate { get; set; }
        public string NoteTitle { get; set; } = "";
        public string NoteContent { get; set; } = "";
    }
}
