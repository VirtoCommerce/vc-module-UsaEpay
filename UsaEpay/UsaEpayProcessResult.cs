namespace UsaEpay
{
    public class UsaEpayProcessResult
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }
        public string PaymentId { get; set; }
    }
}