using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PPB.DAL.Concrete
{
    [Table("Users", Schema = "PPB")]
    public class Users
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string NameSurname { get; set; }
        public int Coin { get; set; } = 100;
        public bool isAdmin { get; set; } = false;
        
    }
}
