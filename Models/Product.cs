using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Team1_Website.Models
{
    public class Product
    {
        public Product()
        {
            Id = new Guid();
        }
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Desc { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public string ImagePath { get; set; } //location

    }
}