
using System.ComponentModel.DataAnnotations;

namespace ShieldMyRide.DTOs.ProposalpolicyDocument
{
    public class ProposalWithDocumentsFormModelDTO
    {
        public int PolicyId { get; set; }
        [Required(ErrorMessage = "Policy name is required")]
        public string PolicyName { get; set; }
        public string VehicleType { get; set; }
        public string VehicleRegNo { get; set; }
        public int VehicleAge { get; set; }
        public decimal CoverageAmount { get; set; }

        // File uploads
        public IFormFile? DrivingLicense { get; set; }
        public IFormFile? PreviousInsurance { get; set; }
        public IFormFile? IncomeProof { get; set; }
        public IFormFile? PassportPhoto { get; set; }
        public IFormFile? AddressProof { get; set; }
        public IFormFile? Signature { get; set; }
    }
}
