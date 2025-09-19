namespace ShieldMyRide.DTOs.UsersDTO
{
    public class UserSearchDTO
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AadhaarMasked { get; set; }
        public string PanMasked { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
