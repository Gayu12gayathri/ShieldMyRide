namespace ShieldMyRide.DTOs.OfficerAssignmentDTO
{
    public class OfficerAdminDTO
    {
        public int AssignmentId { get; set; }
        public int OfficerId { get; set; }
        public int ProposalId { get; set; }
        public int? ClaimId { get; set; }
        public DateTime AssignedAt { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }

        // Computed / mapped from navigation
        public string? OfficerName { get; set; }   // From Officer.FirstName + Officer.LastName
        public string? CustomerName { get; set; } // From Proposal.User.FirstName + LastName
        public string? VehicleRegNo { get; set; }
    }
}
