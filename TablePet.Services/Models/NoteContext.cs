using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace TablePet.Services.Models
{
    public class NoteContext: DbContext
    {
        //需补充数据库连接 
        public NoteContext() :base("name=DbConnectionString")
        {
            Database.SetInitializer<NoteContext>(new CreateDatabaseIfNotExists<NoteContext>());
        }

        public DbSet<Note> Notes { get; set; }
    }
}
