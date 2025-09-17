namespace ShieldMyRide.DTOs.OfficerAssignmentDTO
{
    public class OfficerDTO
    {
        public int AssignmentId { get; set; }
        public int ProposalId { get; set; }
        public int? ClaimId { get; set; }
        public DateTime AssignedAt { get; set; }
        public string? Remarks { get; set; }
        public string? Status { get; set; }   // current status (e.g. Pending, Approved, Rejected)

        // For context
        public string? Username { get; set; }
        public string? VehicleRegNo { get; set; }
        public object AadhaarMasked { get; internal set; }
        public object PanMasked { get; internal set; }
    }
}
