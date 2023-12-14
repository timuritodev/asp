using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;


namespace ASP.Models
{
    public class RegisterViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string SelectedCountry { get; set; }
        public string Gender { get; set; }
    }
}
