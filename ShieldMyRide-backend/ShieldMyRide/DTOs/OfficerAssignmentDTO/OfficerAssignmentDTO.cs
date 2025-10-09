using ShieldMyRide.Models;

namespace ShieldMyRide.DTOs.OfficerAssignmentDTO
{
    public class OfficerAssignmentDTO
    {
        public int OfficerAssignmentId { get; set; }
        public int OfficerId { get; set; }
        public string OfficerName { get; set; }   // 👈 New field
        public int ProposalId { get; set; }
        public int ClaimId { get; set; }
        public string Remarks { get; set; }
        public DateTime AssignedDate { get; set; }
        public OfficerStatus Status { get; set; }
    }
}
