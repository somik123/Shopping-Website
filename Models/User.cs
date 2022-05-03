using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Team1_Website.Models 
{
    public class User
    {
        public User()
        {
            Id = new Guid();
            PurchaseHistory = new List<PurchaseHistory>();
        }
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public virtual ICollection<PurchaseHistory> PurchaseHistory { get; set; }

    }
}