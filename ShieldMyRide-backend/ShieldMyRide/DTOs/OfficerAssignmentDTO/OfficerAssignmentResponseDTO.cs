namespace ShieldMyRide.DTOs.OfficerAssignmentDTO
{
    public class OfficerAssignmentResponseDTO
    {
        public int Id { get; set; }
        public string OfficerId { get; set; }
        public string ActionType { get; set; }
        public string TargetId { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public DateTime ActionTime { get; set; }
    }
}
