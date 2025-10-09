using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ShieldMyRide.Authentication;

namespace ShieldMyRide.Models
{

    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public int ProposalID { get; set; }
        [Required]
        [StringLength(100)]
        public string TransactionId { get; set; } = Guid.NewGuid().ToString();
        [Required,Range(1,double.MaxValue)]
        public decimal AmountPaid { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }
        [Required,StringLength(50)]
        public string? PaymentStatus { get; set; }

        [Required]
        public bool ForClaim { get; set; } = false;
        // Navigation
        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public Proposal? Proposal { get; set; }

    }
}
