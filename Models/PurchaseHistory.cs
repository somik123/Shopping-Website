using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Team1_Website.Models
{
    public class PurchaseHistory
    {
        public PurchaseHistory()
        {
            Id = new Guid();
            PurchaseList = new List<PurchaseList>();
        }
        public Guid Id { get; set; }



        //public int Quantity { get; set; }

        public DateTime PurchaseDate { get; set; }

        public virtual Payment Payment { get; set; }

        public virtual ICollection<PurchaseList> PurchaseList { get; set; }
    }
}
