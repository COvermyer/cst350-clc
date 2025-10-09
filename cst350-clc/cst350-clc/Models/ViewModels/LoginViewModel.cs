using System.ComponentModel.DataAnnotations;

namespace cst350_clc.Models.ViewModels
{
    /// <summary>
    /// Used as a ViewModel to enforce user complies with filling all fields in the login form
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Username associated with the user account
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Password that the user is attempting to use to log in.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
