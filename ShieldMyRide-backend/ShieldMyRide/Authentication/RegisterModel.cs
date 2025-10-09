using System.ComponentModel.DataAnnotations;

namespace ShieldMyRide.Authentication
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Aadhar number is required")]
        public string AadhaarMasked { get; set; }
        [Required(ErrorMessage = "PanNumber  is required")]
        public string PanMasked { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
        
    }
}
