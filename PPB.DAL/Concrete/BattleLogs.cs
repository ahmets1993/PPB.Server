using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PPB.DAL.Concrete
{
    [Table("BattleLogs", Schema = "PPB")]
    public class BattleLogs
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }
        public string UserName { get; set; }
        public int Score { get; set; }

        public string LobbyNo { get; set; }

        public int RoundNo { get; set; }

        public string PlayerMove { get; set; }

        public int roundScore { get; set; }
    }
}
