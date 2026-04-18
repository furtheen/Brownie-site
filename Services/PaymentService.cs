using Razorpay.Api;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace BrownieShop.API.Services
{
    public class PaymentService
    {
        private readonly string _keyId;
        private readonly string _keySecret;

        public PaymentService(IConfiguration config)
        {
            _keyId = config["Razorpay:KeyId"];
            _keySecret = config["Razorpay:KeySecret"];
        }

        public string CreateRazorpayOrder(decimal amount, string receiptId)
        {
            // amount in paise (1 INR = 100 paise)
            int amountInPaise = (int)(amount * 100);

            RazorpayClient client = new RazorpayClient(_keyId, _keySecret);

            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", amountInPaise);
            options.Add("receipt", receiptId);
            options.Add("currency", "INR");
            options.Add("payment_capture", "1");

            Order order = client.Order.Create(options);

            return order["id"].ToString();
        }

        public bool VerifySignature(string razorpayOrderId, string razorpayPaymentId, string razorpaySignature)
        {
            string payload = $"{razorpayOrderId}|{razorpayPaymentId}";
            string expectedSignature = HmacSha256(payload, _keySecret);

            return expectedSignature == razorpaySignature;
        }

        private string HmacSha256(string message, string secret)
        {
            var encoding = new ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);

            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
            }
        }
    }
}
