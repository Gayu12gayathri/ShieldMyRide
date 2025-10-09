using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShieldMyRide.Models
{
    public class OfficersAssignment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string OfficerId { get; set; } // FK to User table

        public string ActionType { get; set; } // e.g., "ProposalReviewed", "QuoteGenerated", "ClaimVerified"

        public string TargetId { get; set; } // ProposalId, QuoteId, ClaimId, etc.

        public string Remarks { get; set; }

        public DateTime ActionTime { get; set; } = DateTime.UtcNow;

        // Optional: Status of the action
        public string Status { get; set; } // e.g., "Pending", "Completed"
    }
}
