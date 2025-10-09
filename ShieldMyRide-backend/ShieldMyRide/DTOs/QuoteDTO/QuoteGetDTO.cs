namespace ShieldMyRide.DTOs.QuoteDTO
{
    public class QuoteGetDTO
    {
        public int QuoteId { get; set; }
        public ICollection<int> ProposalIds { get; set; } = new List<int>();
        public ICollection<int> UserIds { get; set; } = new List<int>();
        public decimal Premium { get; set; }
        public DateTime ValidTill { get; set; }
    }
}
