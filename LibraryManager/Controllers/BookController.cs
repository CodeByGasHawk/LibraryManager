using LibraryManager.Models;
using LibraryManager.Models.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.Controllers;

public class BookController : Controller
{
    private IBookRepository _bookRepository;
    public BookController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository; ;
    }

    public IActionResult AddBook()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddBook(Book book)
    {
        var existingBook = _bookRepository.FindBookByISBN(book.ISBN);
        if (ModelState.IsValid && existingBook is not null)
        {

            ViewBag.AddBookMessage = $"شابک {book.ISBN} با عنوان {existingBook} قبلا ثبت شده است.";
            return View();
        }
        else if (ModelState.IsValid && existingBook is null)
        {
            Normalizer.NormalizeBook(book);
            _bookRepository.CreateBook(book);
            ViewBag.AddBookMessage = $"{book} با موفقیت ثبت شد.";
            return View();
        }
        else
        {
            return View();
        }
    }

    public IActionResult PrintBooks()
    {
        var listOfBooks = _bookRepository.ReadAllBooks().OrderBy(book => book.Title).ToList();
        return View(listOfBooks);
    }

    [HttpPost]
    public IActionResult PrintBooks(string title, string author, string ISBN)
    {

        var searchResult = _bookRepository.SearchBook(title ?? "".ToUpper(), author ?? "".ToUpper(), ISBN ?? "")
            .OrderBy(book => book.Title).ToList();
        return View(searchResult);
    }

    public IActionResult UpdateBook(int id)
    {
        var updatedBook = _bookRepository.FindBookById(id);
        TempData["bookId"] = updatedBook.Id;
        return View(updatedBook);
    }

    [HttpPost]
    public IActionResult UpdateBook(Book book, int bookId)
    {
        var existingBook = _bookRepository.FindBookByISBN(book.ISBN);
        bool updateCondition = (existingBook is not null && existingBook.Id == bookId) || existingBook is null ? true : false;
        if (ModelState.IsValid && updateCondition)
        {
            Normalizer.NormalizeBook(book);
            var updatedBook = _bookRepository.FindBookById(bookId);
            updatedBook.Title = book.Title;
            updatedBook.Author = book.Author;
            updatedBook.ISBN = book.ISBN;
            _bookRepository.UpdateBook(updatedBook);
            TempData["UpdateBookMessage"] = $"{book} با موفقیت تغییر یافت.";
            return RedirectToAction("PrintBooks");
        }
        else if (ModelState.IsValid && !updateCondition)
        {
            ViewBag.UpdateBookMessage = $"شابک {book.ISBN} با عنوان {existingBook} قبلا ثبت شده است.";
            return View(book);
        }
        else
        {
            return View(book);
        }
    }

    public IActionResult DeleteBook(int id)
    {
        var book = _bookRepository.FindBookById(id);
        if(book.IsAvailable != "موجود")
        {
            TempData["UpdateBookMessage"] = $"کتاب به اعضا امانت داده شده است. ابتدا باید به کتابخانه تحویل شود.";
            return RedirectToAction("PrintBooks");
        }
        else
        {
            _bookRepository.DeleteBook(id);
            TempData["UpdateBookMessage"] = $"{book} با موفقیت حذف شد.";
            return RedirectToAction("PrintBooks");
        }
        
    }

}
