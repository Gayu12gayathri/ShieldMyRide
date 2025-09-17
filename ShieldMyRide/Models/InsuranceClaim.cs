using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ShieldMyRide.Authentication;

namespace ShieldMyRide.Models
{
    public class InsuranceClaim
    {
        [Key]
        public int ClaimId { get; set; }
        [Required]
        public int ProposalId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime ClaimDate { get; set; }
        [Required,StringLength(500)]
        public string? ClaimDescription { get; set; }
        [Required, Range(1, double.MaxValue)]
        public decimal ClaimAmount { get; set; }
        [Required, StringLength(50)]
        public string? ClaimStatus { get; set; }
        [Range(0, double.MaxValue)]

        public decimal SettlementAmount { get; set; }

        // Navigation
        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public Proposal? Proposal { get; set; }
        [JsonIgnore]
        public ICollection<OfficerAssignment>? OfficerAssignments { get; set; } = new List<OfficerAssignment>();
    }
}
