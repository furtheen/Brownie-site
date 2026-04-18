namespace BrownieShop.API.DTOs
{
    public class PaymentVerificationRequest
    {
        public string RazorpayPaymentId { get; set; }
        public string RazorpayOrderId { get; set; }
        public string RazorpaySignature { get; set; }
        public int LocalOrderId { get; set; }
    }
}
