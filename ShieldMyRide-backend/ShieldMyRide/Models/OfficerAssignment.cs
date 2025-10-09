using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ShieldMyRide.Authentication;

namespace ShieldMyRide.Models
{
    public enum OfficerStatus
    {
        Assigned = 0,     // Admin has assigned officer
        InProgress = 1,   // Officer is working
        Reviewed = 2,     // Officer has submitted review
        Approved = 3,     // Admin approved review
        Rejected = 4,     // Admin rejected review
        Closed = 5,     // Case closed
        Pending =6
    }
    public class OfficerAssignment
    {
        [Key]
        public int OfficerAssignmentId { get; set; }
        public int OfficerId { get; set; }
        [Required]
        public int ProposalId { get; set; }
        [Required]
        public int ClaimId { get; set; }
        [Required, MaxLength(500)]
        public string? Remarks { get; set; }
        [Required]
        public DateTime AssignedDate { get; set; }
        //[Required, MaxLength(50)]

        public OfficerStatus Status { get; set; }

        // Navigation
        [JsonIgnore]
        public User? Officer { get; set; }
        [JsonIgnore]
        public Proposal? Proposal { get; set; }
        [JsonIgnore]
        public InsuranceClaim? Claim { get; set; }

    }
}
