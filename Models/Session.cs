using System;
using System.ComponentModel.DataAnnotations;


namespace Team1_Website.Models
{
    public class Session
    {
        public Session()
        {
            Id = new Guid();

            Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        }
        public Guid Id { get; set; }

        public long Timestamp { get; set; }

        public virtual User User { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}
