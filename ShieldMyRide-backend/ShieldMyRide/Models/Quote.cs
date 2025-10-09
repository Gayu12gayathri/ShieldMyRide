using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShieldMyRide.Models
{
    public class Quote
    {
        [Key]
        public int QuoteId { get; set; }
        [Required]
        public int ProposalId { get; set; }
        [Required]
        public int PolicyId { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime DateIssued { get; set; }

        [Required, Range(1, double.MaxValue)]
        public decimal PremiumAmount { get; set; }

        [Required, StringLength(500)]
        public string CoverageDetails { get; set; }

        [Required]
        public DateTime GeneratedAt { get; set; }

        [Required]
        public DateTime ValidTill { get; set; }

        // Navigation
        [JsonIgnore]
        public Policy? Policy { get; set; }
        
        // Quote.cs
        [JsonIgnore]
        public Proposal? Proposal { get; set; }

        //public ICollection<Proposal>? Proposal { get; set; } = new List<Proposal>();
        [JsonIgnore]

        public ICollection<Payment>? Payments { get; set; }
    }
}
