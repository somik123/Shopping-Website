using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Team1_Website.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Id = new Guid();
            CartItems = new List<CartItem>();
        }
        public Guid Id { get; set; }

        public virtual Payment Payment { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
