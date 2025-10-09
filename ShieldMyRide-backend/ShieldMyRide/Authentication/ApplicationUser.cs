using Microsoft.AspNetCore.Identity;
using ShieldMyRide.Models;

namespace ShieldMyRide.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string? AadhaarHash { get; set; } 
        public string? PanHash { get; set; } 
    }
}
