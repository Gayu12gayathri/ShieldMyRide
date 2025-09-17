namespace ShieldMyRide.DTOs.PolicyDTO
{
    public class PolicyGetDTO
    {
        public int PolicyId { get; set; }
        public string? PolicyName { get; set; }
        public string? CoverageType { get; set; }
        public decimal BasePremium { get; set; }
        public decimal CoverageAmount { get; set; }
        public int DurationMonths { get; set; }
        public ICollection<int> ProposalIds { get; set; } = new List<int>();
        public ICollection<PolicyDocumentDTO> Documents { get; set; } = new List<PolicyDocumentDTO>();

    }
}
