using System;
using System.Runtime.Serialization;

namespace UsaEpay.Communication
{
    [DataContract]
    public class AddPaymentMethodRequest
    {
        [DataMember]
        public Guid ApplicationId { get; set; }

        [DataMember]
        public string AccountIdentifier { get; set; }

        [DataMember]
        public PaymentMethodType PaymentMethodType { get; set; }

        [DataMember]
        public DateTime ExpirationDate { get; set; }

        [DataMember]
        public string CardVerificationValue { get; set; }

        [DataMember]
        public string CustomerId { get; set; }
    }
}
