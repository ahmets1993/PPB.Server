using PPB.DAL.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPB.Client.Models
{
     public class UserAuthentication
    {
        public bool Auth { get; set; }

        public bool isAdmin { get; set; }
        public bool PackageStatus { get; set; }
        public string Response { get; set; }
    }
}
