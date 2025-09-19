using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ShieldMyRide.Authentication;

namespace ShieldMyRide.Models
{
    public enum ProposalStatus
    {
        Submitted,
        QuoteGenerated,
        Rejected,
        Active,
        Approved
    }
    public class Proposal
    {
        [Key]
        public int ProposalId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int PolicyId { get; set; }
        //public int QuoteId { get; set; }

        [Required(ErrorMessage = "Vehicle type is required")]
        [StringLength(50, ErrorMessage = "Vehicle type cannot exceed 50 characters")]
        public string? VehicleType { get; set; }

        [Required(ErrorMessage = "Vehicle registration number is required")]
        [RegularExpression(@"^[A-Z]{2}[0-9]{2}[A-Z]{1,2}[0-9]{4}$",
        ErrorMessage = "Invalid vehicle registration number format (e.g., TN09AB1234)")]
        public string? VehicleRegNo { get; set; }

        [Range(0, 50, ErrorMessage = "Vehicle age must be between 0 and 50 years")]
        public int VehicleAge { get; set; }
        [Required]
        [Range(1000, 1000000, ErrorMessage = "Premium must be between 1000 and 1000000")]
        public decimal Premium { get; set; }

        [Required(ErrorMessage = "Status is required")]
        //[ ErrorMessage = "Status cannot exceed 20 characters"]
        public ProposalStatus ProposalStatus { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        // Navigation
        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]

        public Policy? Policy { get; set; }   // ✅ template policy (optional)

        [JsonIgnore]
        public ICollection<Quote> Quotes { get; set; } = new List<Quote>(); 

        [JsonIgnore]
        public ICollection<PolicyDocument>? PolicyDocuments { get; set; }
        [JsonIgnore]
        public ICollection<InsuranceClaim>? Claims { get; set; }
        [JsonIgnore]
        public ICollection<OfficerAssignment>? OfficerAssignments { get; set; }
    }

}
