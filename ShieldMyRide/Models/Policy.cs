using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShieldMyRide.Models
{
    public class Policy
    {
        [Key]
            public int PolicyId { get; set; }
            [Required, StringLength(100)]
            public string? PolicyName { get; set; }
            [StringLength(500)]
            public string? Description { get; set; }
            [Required, StringLength(50)]
            public string? CoverageType { get; set; }
            [Required, Range(100, double.MaxValue)]
            public decimal BasePremium { get; set; }

            [Required, Range(1, int.MaxValue)]
            public int DurationMonths { get; set; }

            [Required, Range(1000, double.MaxValue)]
            public decimal CoverageAmount { get; set; }

            public DateTime CreatedAt { get; set; }
            public int CreatedBy { get; set; }
            public DateTime ModifiedAt { get; set; }
            public int? ModifiedBy { get; set; }

        // Navigation
        [JsonIgnore]
        public ICollection<Proposal>? Proposals { get; set; }
        [JsonIgnore]
        public ICollection<Quote>? Quotes { get; set; } = new List<Quote>();
        [JsonIgnore]
        public ICollection<PolicyDocument>? PolicyDocuments { get; set; }
    }

}
