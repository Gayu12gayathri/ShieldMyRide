using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace ShieldMyRide.Models
{
    public class User : IValidatableObject
    {
        public int UserId { get; set; }
        //public string? IdentityUserId { get; set; }
        public string IdentityUserId { get; set; }


        [Required(ErrorMessage = "First Name is required"), StringLength(50)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required"), StringLength(50)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required"),
         EmailAddress(ErrorMessage = "Invalid Email Address"),
         StringLength(120)]
        public string? Email { get; set; }

        [Required, MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string? PasswordHash { get; set; }

        [Required(ErrorMessage = "Date of Birth is required"), DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required, RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string? PhoneNumber { get; set; }

        [Required, RegularExpression(@"^\d{12}$", ErrorMessage = "Aadhaar must be 12 digits")]
        public string? AadhaarNumber { get; set; }

        [Required, RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid PAN format")]
        public string? PanNumber { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string? Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        [JsonIgnore]
        public ICollection<Proposal>? Proposals { get; set; }
        [JsonIgnore]
        public ICollection<Payment>? Payments { get; set; }
        [JsonIgnore]
        public ICollection<InsuranceClaim>? Claims { get; set; }
        [JsonIgnore]
        public ICollection<OfficerAssignment>? OfficerAssignments { get; set; }

        // Custom validation logic
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Age validation: must be at least 18
            if (DateOfBirth > DateTime.Now.AddYears(-18))
            {
                yield return new ValidationResult(
                    "User must be at least 18 years old",
                    new[] { nameof(DateOfBirth) });
            }
        }
    }
}

