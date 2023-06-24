using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager.Models;

public class Member
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(50, ErrorMessage = "حداکثر 50 کاراکتر"),Required(ErrorMessage ="نام را وارد کنید")]
    public string FirstName { get; set; }

    [MaxLength(50,ErrorMessage = "حداکثر 50 کاراکتر"), Required(ErrorMessage = "نام خانوادگی را وارد کنید")]
    public string LastName { get; set; }

    [RegularExpression("^\\d{10}$", ErrorMessage = "کد ملی صحیح نیست"), Required(ErrorMessage = "کد ملی را وارد کنید")]
    public string NationalCode { get; set; }
    public List<BorrowedBookDetails> BorrowedBooksHistory { get; set; } = new();

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}
