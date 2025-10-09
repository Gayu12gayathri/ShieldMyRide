using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShieldMyRide.Models
{
    public class PolicyDocument
    {
        [Key]
        public int DocumentId { get; set; }
        [Required]
        public int ProposalId { get; set; }
        public int? PolicyId { get; set; }

        [Required(ErrorMessage = "Document path is required")]
        [StringLength(200, ErrorMessage = "Document path cannot exceed 200 characters")]
        public string? DocumentPath { get; set; }

        [Required(ErrorMessage = "Document type is required")]
        [StringLength(50, ErrorMessage = "Document type cannot exceed 50 characters")]
        public string DocumentType { get; set; }
        public DateTime UploadedAt { get; set; }

        [Required(ErrorMessage = "Issued date is required")]
        public DateTime IssuedDate { get; set; }

        // Navigation
        [JsonIgnore]
        public Proposal? Proposal { get; set; }
        [JsonIgnore]
        public Policy? Policy { get; set; }
    }
}
