namespace ShieldMyRide.DTOs.QuoteDTO
{
    public class QuotePostDTO
    {
        public string UserId { get; set; } = string.Empty;
        public int ProposalId { get; set; }   // ✅ link to proposal
        public int PolicyId { get; set; }     // ✅ link to policy
        public decimal Premium { get; set; }
        public string CoverageDetails { get; set; } = string.Empty;
    }
}
