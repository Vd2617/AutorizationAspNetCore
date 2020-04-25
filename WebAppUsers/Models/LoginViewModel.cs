using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppUsers.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
        
    
    }
}
