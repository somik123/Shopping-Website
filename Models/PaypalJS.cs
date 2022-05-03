namespace Team1_Website.Models
{
    public class PaypalJS
    {
        public string name { get; set; }
        public string description { get; set; }

        public UnitAmount unit_amount { get; set; }
        public string quantity { get; set; }
    }


    public class UnitAmount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }
}
