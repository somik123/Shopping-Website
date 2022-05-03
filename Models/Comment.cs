using System;
namespace Team1_Website.Models
{
	public class Comment
	{
		public Comment()
		{
			Id = new Guid();
		}

		public Guid Id { get; set; }
		public virtual User User { get; set; }
		public virtual Product Product { get; set; }
		public string CommentString { set; get; }
	}
}

