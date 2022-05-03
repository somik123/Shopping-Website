using System;
using System.Collections.Generic;

namespace Team1_Website.Models
{
    public class PurchaseList
    {
        public Guid Id { get; set; }
        public PurchaseList()
        {
            Id = new Guid();
            ActivationCodes = new List<ActivationCode>();
        }

        public string DownloadLink { get; set; }

        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<ActivationCode> ActivationCodes { get; set; } 

    }
}
