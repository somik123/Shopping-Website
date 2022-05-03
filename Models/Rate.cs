using System;
namespace Team1_Website.Models
{
	public class Rate
	{
		public Rate()
		{
			Id = new Guid();
		}

		public Guid Id { get; set; }
		public virtual User User { get; set; }
		public virtual Product Product { get; set; }
		public double Stars { set; get; }
	}
}

