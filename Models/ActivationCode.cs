using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Team1_Website.Models
{
    public class ActivationCode
    {
        public ActivationCode()
        {
            Id = new Guid();
        }
        public Guid Id { get; set; }
    }
}
