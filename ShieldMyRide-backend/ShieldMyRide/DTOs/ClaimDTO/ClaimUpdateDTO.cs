namespace ShieldMyRide.DTOs.ClaimsDTO
{
    public class ClaimUpdateDTO
    {
        public string ClaimStatus { get; set; } = string.Empty;
        public decimal SettlementAmount { get; set; }
    }
}
