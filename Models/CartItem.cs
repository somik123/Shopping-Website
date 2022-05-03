using System;
using System.ComponentModel.DataAnnotations;

namespace Team1_Website.Models
{
    public class CartItem
    {
        public CartItem()
        {
            Id = new Guid();
        }
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public virtual Product Product { get; set; }
    }
}
