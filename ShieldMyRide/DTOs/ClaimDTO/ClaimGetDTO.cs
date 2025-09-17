namespace ShieldMyRide.DTOs.ClaimDTO
{
    public class ClaimGetDTO
    {
        public int ClaimId { get; set; }
        public int ProposalId { get; set; }
        public int UserId { get; set; }
        public DateTime ClaimDate { get; set; }
        public string ClaimDescription { get; set; } = string.Empty;
        public decimal ClaimAmount { get; set; }
        public string ClaimStatus { get; set; } = string.Empty;
        public decimal SettlementAmount { get; set; }
    }
}
