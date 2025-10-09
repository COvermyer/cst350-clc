using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace cst350_clc.Models.ViewModels
{
    // Internal class
    public class GroupViewModel
    { 
        public bool IsSelected { get; set; }
        public string GroupName { get; set; }
    }

    public class RegisterViewModel
    {
        // Properties for RegisterViewModel class
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public List<GroupViewModel> Groups { get; set; }

        // Constructor
        public RegisterViewModel()
        {
            FirstName = "";
            LastName = "";
            Age = 13; // Min age for site
            Email = "";
            State = "";            
            Username = "";
            Password = "";

            Groups = new List<GroupViewModel>()
            {
                new GroupViewModel { GroupName = "ADMIN", IsSelected = false },
                new GroupViewModel { GroupName = "USER", IsSelected = false }
            };
        }

        public List<SelectListItem> GetStates() => new()
        {
            new SelectListItem { Value = "", Text = "Select a state" },
            new SelectListItem { Value = "AL", Text = "Alabama" },
            new SelectListItem { Value = "AK", Text = "Alaska" },
            new SelectListItem { Value = "AZ", Text = "Arizona" },
            new SelectListItem { Value = "AR", Text = "Arkansas" },
            new SelectListItem { Value = "CA", Text = "California" },
            new SelectListItem { Value = "CO", Text = "Colorado" },
            new SelectListItem { Value = "CT", Text = "Connecticut" },
            new SelectListItem { Value = "DE", Text = "Delaware" },
            new SelectListItem { Value = "FL", Text = "Florida" },
            new SelectListItem { Value = "GA", Text = "Georgia" },
            new SelectListItem { Value = "HI", Text = "Hawaii" },
            new SelectListItem { Value = "ID", Text = "Idaho" },
            new SelectListItem { Value = "IL", Text = "Illinois" },
            new SelectListItem { Value = "IN", Text = "Indiana" },
            new SelectListItem { Value = "IA", Text = "Iowa" },
            new SelectListItem { Value = "KS", Text = "Kansas" },
            new SelectListItem { Value = "KY", Text = "Kentucky" },
            new SelectListItem { Value = "LA", Text = "Louisiana" },
            new SelectListItem { Value = "ME", Text = "Maine" },
            new SelectListItem { Value = "MD", Text = "Maryland" },
            new SelectListItem { Value = "MA", Text = "Massachusetts" },
            new SelectListItem { Value = "MI", Text = "Michigan" },
            new SelectListItem { Value = "MN", Text = "Minnesota" },
            new SelectListItem { Value = "MS", Text = "Mississippi" },
            new SelectListItem { Value = "MO", Text = "Missouri" },
            new SelectListItem { Value = "MT", Text = "Montana" },
            new SelectListItem { Value = "NE", Text = "Nebraska" },
            new SelectListItem { Value = "NV", Text = "Nevada" },
            new SelectListItem { Value = "NH", Text = "New Hampshire" },
            new SelectListItem { Value = "NJ", Text = "New Jersey" },
            new SelectListItem { Value = "NM", Text = "New Mexico" },
            new SelectListItem { Value = "NY", Text = "New York" },
            new SelectListItem { Value = "NC", Text = "North Carolina" },
            new SelectListItem { Value = "ND", Text = "North Dakota" },
            new SelectListItem { Value = "OH", Text = "Ohio" },
            new SelectListItem { Value = "OK", Text = "Oklahoma" },
            new SelectListItem { Value = "OR", Text = "Oregon" },
            new SelectListItem { Value = "PA", Text = "Pennsylvania" },
            new SelectListItem { Value = "RI", Text = "Rhode Island" },
            new SelectListItem { Value = "SC", Text = "South Carolina" },
            new SelectListItem { Value = "SD", Text = "South Dakota" },
            new SelectListItem { Value = "TN", Text = "Tennessee" },
            new SelectListItem { Value = "TX", Text = "Texas" },
            new SelectListItem { Value = "UT", Text = "Utah" },
            new SelectListItem { Value = "VT", Text = "Vermont" },
            new SelectListItem { Value = "VA", Text = "Virginia" },
            new SelectListItem { Value = "WA", Text = "Washington" },
            new SelectListItem { Value = "WV", Text = "West Virginia" },
            new SelectListItem { Value = "WI", Text = "Wisconsin" },
            new SelectListItem { Value = "WY", Text = "Wyoming" }
        };
    }
}
