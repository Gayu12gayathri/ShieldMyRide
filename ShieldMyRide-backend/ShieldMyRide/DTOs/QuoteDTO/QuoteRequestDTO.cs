using System.ComponentModel.DataAnnotations;

namespace ShieldMyRide.DTOs.QuoteDTO
{
    public class QuoteRequestDTO
    {
        [Required]
        public string VehicleType { get; set; } = "car"; // e.g., car, bike, etc.

        [Required]
        [Range(0, 50, ErrorMessage = "Vehicle age must be between 0 and 50 years.")]
        public int VehicleAge { get; set; }

        [Required]
        [Range(1000, double.MaxValue, ErrorMessage = "Coverage amount must be greater than 1000.")]
        public decimal CoverageAmount { get; set; }

        public bool ZeroDep { get; set; } = false; // Optional add-on
        public bool RoadsideAssist { get; set; } = false; // Optional add-on

        [Range(0, 50, ErrorMessage = "NCB percent must be between 0 and 50.")]
        public decimal NCBPercent { get; set; } = 0; // No Claim Bonus
    }
}
