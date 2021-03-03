using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PPB.DAL.Concrete
{
    [Table("Musics", Schema = "PPB")]
    public class Musics
    {
        [Key]
        public int _id { get; set; }
        public string FilePath { get; set; }

    }
}
