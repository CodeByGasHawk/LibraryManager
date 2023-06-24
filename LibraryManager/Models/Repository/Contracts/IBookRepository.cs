namespace LibraryManager.Models.Repository.Contracts;

public interface IBookRepository
{
    public void CreateBook(Book book);
    public Book? FindBookByISBN(string ISBN);
    public Book? FindBookById(int id);
    public List<Book> ReadAllBooks();
    public List<Book>? SearchBook(string title, string author,string ISBN);
    public List<Book> ReadMemberBooks(string memberName);
    public void UpdateBook(Book book);
    public void DeleteBook(int id);
}
