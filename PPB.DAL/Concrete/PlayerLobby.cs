using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPB.DAL.Concrete
{
    [Table("PlayerLobby", Schema = "PPB")]
    public class PlayerLobby
    {
        [Key]
        public int ID { get; set; }
        public string UserName { get; set; }
        public string LobbyNo { get; set; }
        public int Score { get; set; } = 10000; 
        public bool Status { get; set; } = false;
    }
}
