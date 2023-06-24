using LibraryManager.Models.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Models.Repository;

public class BookRepository : IBookRepository
{
    private readonly Contexts _context;

    public BookRepository(Contexts context)
    {
        _context = context;
    }

    //Create
    public void CreateBook(Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();
    }
    //Read
    public Book? FindBookByISBN (string ISBN)
    {
        return _context.Books.ToList().Find(book => book.ISBN == ISBN);
    }
    public Book? FindBookById(int id)
    {
        return _context.Books.Find(id);
    }
    public List<Book> ReadAllBooks()
    {
        return _context.Books.ToList();
    }
    public List<Book>? SearchBook(string title,string author,string ISBN)
    {
        return _context.Books
            .Where(book => book.Title.ToUpper().Contains(title) &&
                           book.Author.ToUpper().Contains(author) &&
                           book.ISBN.Contains(ISBN)).ToList();
    }

    public List<Book> ReadMemberBooks(string memberName)
    {
        if (memberName is not null)
        {
            return _context.Books.Where(book => book.Member == memberName).ToList();
        }
        else
        {
            return null;
        }
    }
    //Update
    public void UpdateBook(Book book)
    {
        _context.Entry(book).State = EntityState.Modified;
        _context.SaveChanges();
    }
    //Delete
    public void DeleteBook(int id)
    {
        Book deletedBook = _context.Books.Find(id);
        _context.Books.Remove(deletedBook);
        _context.SaveChanges();
    }
}
