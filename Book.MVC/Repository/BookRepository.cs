using Book.MVC.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Book.MVC.Repository
{
    public class BookRepository
    {
        private string _connectionString = "Server=DESKTOP-9QS312H\\SQLEXPRESS01; Database=BookLibrary; TrustServerCertificate=True;Integrated Security=True;";

        public List<Books> GetBooks()
        {
            List<Books> books = new List<Books>();

            // 1. Kreiramo novu konekciju
            SqlConnection connection = new SqlConnection(_connectionString);

            // 2. Kreiramo novu komandu
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Book";

            // 3. Otvaramo konekciju
            connection.Open();

            // 4. Čitanje podataka
            SqlDataReader readBooks = cmd.ExecuteReader();

            while (readBooks.Read())
            {
                Books book = new Books();

                // 5. Mapiranje podataka iz tablice baze podataka
                book.Id = (int)readBooks["Id"];
                book.Title = readBooks["Title"].ToString();
                book.Description = readBooks["Description"].ToString();
                book.Genre = readBooks["Genre"].ToString();
                book.Stock = (int)readBooks["Stock"];
                book.ReleaseDate = (DateTime)readBooks["ReleaseDate"];
                book.Author = new Author();
                book.Author.Id = (int)readBooks["AuthorId"];

                books.Add(book);
            }

            // 6. Zatvoriti konekciju
            // ako reader nije zatvoren zatvori ga
            if (!readBooks.IsClosed)
            {
                readBooks.Close();
            }

            connection.Close();
            connection.Dispose();

            return books;
        }

        public Books GetBookById(int id)
        {
            Books book = new Books();

            // 1. Kreiramo konekciju - skraćeni način, automatski zatvara konekciju
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // 2. Kreiramo komandu (plus parametre)
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT * FROM Book WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", id);

                // 3. Otvaramo konekciju
                connection.Open();

                // 4. Čitamo podatke
                SqlDataReader readBook = cmd.ExecuteReader();
                while (readBook.Read())
                {
                    // 5. Mapiramo podatke
                    book.Id = (int)readBook["Id"];
                    book.Title = readBook["Title"].ToString();
                    book.Description = readBook["Description"].ToString();
                    book.Genre = readBook["Genre"].ToString();
                    book.Stock = (int)readBook["Stock"];
                    book.ReleaseDate = (DateTime)readBook["ReleaseDate"];
                    book.Author = new Author();
                    book.Author.Id = (int)readBook["AuthorId"];
                }

                readBook.Close();

                // 5.1 Dohvatiti i povezati autora

                using (var cmdItems = new SqlCommand("SELECT * FROM Author WHERE Id = @Id",
                    connection))
                {
                    cmdItems.Parameters.AddWithValue("@Id", id);

                    using (var readerAuthor = cmdItems.ExecuteReader())
                    {
                        while (readerAuthor.Read())
                        {
                            book.Author.Name = readerAuthor["Name"].ToString();
                            book.Author.Bio = readerAuthor["Bio"].ToString();
                        }
                    }
                }
                // 6. Zatvaramo konekciju
            }


            return book;
        }

        public void DeleteBook(int bookId)
        {
            // 1. Kriranje konekcije
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // 2. otvaranje konekcije
                connection.Open();
                // brisanje knjige
                using (var cmdItems = new SqlCommand("DELETE FROM Book WHERE Id=@Id",
                    connection))
                {
                    cmdItems.Parameters.AddWithValue("@Id", bookId);

                    cmdItems.ExecuteNonQuery();
                }
            }
        }
    }
}
