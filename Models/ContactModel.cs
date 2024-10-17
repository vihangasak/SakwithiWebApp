using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SakwithiWebApp.Models
{
    public class ContactModel
    {
        [Required(ErrorMessage = "First name is required")]
        [Display(Name ="First name")]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last name")]
        public String LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public String Message { get; set; }

        [Required(ErrorMessage = "Answer is required")]
        [Display(Name = "I am not a Robot")]
        public Boolean NotRbot { get; set; }
    }
}
