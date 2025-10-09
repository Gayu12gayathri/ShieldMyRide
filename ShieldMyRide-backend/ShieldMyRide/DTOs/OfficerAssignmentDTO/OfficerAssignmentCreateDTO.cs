namespace ShieldMyRide.DTOs.OfficerAssignmentDTO
{
    public class OfficerAssignmentCreateDTO
    {
        public string OfficerId { get; set; }
        public string ActionType { get; set; }
        public string TargetId { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
    }
}
