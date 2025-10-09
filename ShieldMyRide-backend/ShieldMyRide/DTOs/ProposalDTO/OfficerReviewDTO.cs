using ShieldMyRide.Models;

namespace ShieldMyRide.DTOs.ProposalDTO
{
    public class OfficerReviewDTO
    {
        public ProposalStatus ProposalStatus { get; set; }   // Approved, Rejected,Pending
        public string? Remarks { get; set; }
    }
}
