using ShieldMyRide.Models;

namespace ShieldMyRide.DTOs.ProposalDTO
{
    public class OfficerReviewDTO
    {
        public ProposalStatus ProposalStatus { get; set; }   // Approved, Rejected, etc.
        public string? Remarks { get; set; }
    }
}
