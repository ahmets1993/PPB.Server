using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PPB.DAL.Concrete
{
    [Table("UserMusics", Schema = "PPB")]

    public class UserMusics
    {
        [Key]
        public int _id { get; set; }
        public int UserID { get; set; }
        public int MusicID { get; set; }
    }
}
