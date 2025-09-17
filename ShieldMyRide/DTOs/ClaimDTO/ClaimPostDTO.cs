namespace ShieldMyRide.DTOs.ClaimDTO
{
    public class ClaimPostDTO
    {
        public int ProposalId { get; set; }
        public int UserId { get; set; }
        public DateTime ClaimDate { get; set; }
        public string ClaimDescription { get; set; } = string.Empty;
        public decimal ClaimAmount { get; set; }
    }
}
