namespace ShieldMyRide.DTOs.PolicyDTO
{
    public class PolicyDocumentDTO
    {
        public int DocumentId { get; set; }
        public string? DocumentPath { get; set; }
        public string? DocumentType { get; set; }
        public DateTime IssuedDate { get; set; }
    }
}
