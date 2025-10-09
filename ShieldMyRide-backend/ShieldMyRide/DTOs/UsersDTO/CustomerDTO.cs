namespace ShieldMyRide.DTOs.UsersDTO
{
    public class CustomerDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string AadhaarMasked { get; set; }
        public string PanMasked { get; set; }
    }
}
