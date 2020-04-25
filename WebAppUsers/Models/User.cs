using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppUsers.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            RegistrationDate = new DateTime();
           
            LastLoginingDate = new DateTime();

            RegistrationDate = DateTime.Now;
           
           
            
            

        }

       

        public DateTime RegistrationDate { get; set; }

        public DateTime LastLoginingDate { get; set; }
       





    }
}
