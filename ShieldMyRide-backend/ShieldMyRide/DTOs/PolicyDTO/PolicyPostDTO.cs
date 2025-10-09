namespace ShieldMyRide.DTOs.PolicyDTO
{
    public class PolicyPostDTO
    {
        public string PolicyName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CoverageType { get; set; } = string.Empty;
        public decimal BasePremium { get; set; }
        public int DurationMonths { get; set; }
        public decimal CoverageAmount { get; set; }
    }
}
