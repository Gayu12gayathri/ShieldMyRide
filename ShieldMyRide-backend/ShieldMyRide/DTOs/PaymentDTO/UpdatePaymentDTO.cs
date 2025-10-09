namespace ShieldMyRide.DTOs.PaymentDTO
{
    public class UpdatePaymentDTO
    {
        public decimal? AmountPaid { get; set; }
        public string TransactionId { get; set; }
        public string PaymentStatus { get; set; }
    }
}
