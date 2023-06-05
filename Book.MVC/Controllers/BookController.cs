using Book.MVC.Models;
using Book.MVC.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Book.MVC.Controllers
{
	public class BookController : Controller
	{
		BookRepository _bookRepository;
		public IActionResult Index()
		{
			List<Books> books = _bookRepository.GetBooks();

			return View(books);
		}

		public IActionResult Details(int id)
		{
			Books book = _bookRepository.GetBookById(id);

			return View(book);
		}

        public IActionResult Delete(int id)
        {
            Books book = _bookRepository.GetBookById(id);
            
            return View(book);
        }

        [HttpPost]
        public IActionResult DeleteBook(int id)
        {
            try
            {
                _bookRepository.DeleteBook(id);
                TempData["Success"] = "Uspješno ste obrisali knjigu!";
            }
            catch
            {
                TempData["Error"] = "Dogodila se greška. Knjiga nije obrisana!";
            }

            return RedirectToAction("Index");
        }
    }
}
