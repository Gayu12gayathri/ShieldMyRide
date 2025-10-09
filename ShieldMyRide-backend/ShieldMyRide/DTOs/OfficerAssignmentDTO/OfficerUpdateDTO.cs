using ShieldMyRide.Models;

namespace ShieldMyRide.DTOs.OfficerAssignmentDTO
{
    public class OfficerUpdateDTO
    {
        public OfficerStatus Status { get; set; }
        public string? Remarks { get; set; }
    }
}
