using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManager.Models;

public class BorrowedBookDetails
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public Book BorrowedBook { get; set; }
    public DateTime TakeDate { get; set; }
    public DateTime ReturnDate { get; set; } = DateTime.Parse("0001-01-01 12:00:00");
    public string Status { get; set; } = "تحویل نشده";
}
