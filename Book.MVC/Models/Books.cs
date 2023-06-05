using System.ComponentModel;

namespace Book.MVC.Models
{
	public class Books
	{
		public int Id { get; set; }
		[DisplayName("Naslov")]
		public string Title { get; set; }
		[DisplayName("Opis")]
		public string Description { get; set; }
		[DisplayName("Žarn")]
		public string Genre { get; set; }
		[DisplayName("Količina")]
		public int Stock { get; set; }
		[DisplayName("Datim izdavanja")]
		public DateTime ReleaseDate { get; set; }
		[DisplayName("Autor")]
		public Author Author { get; set; }
	}
}
