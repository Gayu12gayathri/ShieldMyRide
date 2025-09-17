using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ShieldMyRide.Authentication;

namespace ShieldMyRide.Models
{
    public class OfficerAssignment
    {
        [Key]
        public int OfficerAssignmentId { get; set; }
        public int OfficerId { get; set; }
        [Required]
        public int ProposalId { get; set; }
        [Required]
        public int ClaimId { get; set; }
        [Required,MaxLength(500)]
        public string? Remarks { get; set; }
        [Required]
        public DateTime AssignedDate { get; set; }
        [Required, MaxLength(50)]

        public string? Status { get; set; }

        // Navigation
        [JsonIgnore]
        public User ? Officer { get; set; }
        [JsonIgnore]
        public Proposal? Proposal { get; set; }
        [JsonIgnore]
        public InsuranceClaim? Claim { get; set; }

    }
}
