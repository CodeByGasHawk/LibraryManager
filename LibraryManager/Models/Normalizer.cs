namespace LibraryManager.Models;

public abstract class Normalizer
{
    public static string NormalizeName(string name)
    {
        string trimmedName = name.Trim();
        string normalizedName = string.Concat(char.ToUpper(trimmedName[0]), trimmedName.Substring(1).ToLower());
        return normalizedName;
    }

    public static void NormalizeBook(Book book)
    {
        book.Title = book.Title.Trim();
        book.Author = book.Author.Trim();
    }

    public static void NormalizeMember(Member member)
    {
        member.FirstName = NormalizeName(member.FirstName);
        member.LastName = NormalizeName(member.LastName);
    }
}
