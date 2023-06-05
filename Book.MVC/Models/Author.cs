using System.ComponentModel;

namespace Book.MVC.Models
{
	public class Author
	{
		public int Id { get; set; }
		[DisplayName("Ime")]
		public string Name { get; set; }
		[DisplayName("Biografija")]
		public string Bio { get; set; }
	}
}
