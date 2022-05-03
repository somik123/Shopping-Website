using System;

namespace Team1_Website.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Payment()
        {
            Id = new Guid();
            PaymentConfirm = false;
        }
        public string TransactionCode { get; set; }
        public string ClientResponse { get; set; }
        public string ServerResponse { get; set; }
        public bool PaymentConfirm { get; set; }
    }
}
