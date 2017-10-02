namespace UsaEpay.Communication
{
    public class PaymentMethodDto
    {
        public long ApplicationId { get; set; }

        public string ExternalIdentifier { get; set; }

        public long Id { get; set; }

        public string ApplicationCustomerId { get; set; }

        public PaymentMethodType PaymentMethodType { get; set; }

        public string ExpirationDate { get; set; }

        public string AccountNumber { get; set; }

        public string CardType { get; set; }

        public string CardVerificationValue { get; set; }
    }
}
