using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager.Models;

public class Book
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [RegularExpression("^\\d{13}$", ErrorMessage = "شابک صحیح نیست"), Required(ErrorMessage = "شابک را وارد کنید")]
    public string ISBN { get; set; }

    [MaxLength(100, ErrorMessage = "حداکثر 100 کاراکتر"),Required(ErrorMessage ="عنوان را وارد کنید")]
    public string? Title { get; set; }

    [MaxLength(50, ErrorMessage = "حداکثر 50 کاراکتر"),Required(ErrorMessage ="نویسنده را وارد کنید")]
    public string? Author { get; set; }

    [MaxLength(50, ErrorMessage = "حداکثر 50 کاراکتر")]
    public string? Member { get; set; } = "";
    public string? IsAvailable { get; set; } = "موجود";

    public override string ToString()
    {
        return $"\"{Title}\" اثر \"{Author}\"";
    }
}
