using LibraryManager.Models;
using LibraryManager.Models.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace LibraryManager.Controllers;

public class LibraryController : Controller
{
    private IBookRepository _bookRepository;
    private IMemberRepository _memberRepository;
    public LibraryController(IBookRepository bookRepository, IMemberRepository memberRepository)
    {
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
    }

    public IActionResult Index()
    {
        TempData["Date"] = PersianCal();
        TempData["Time"] = $"{DateTime.Now.TimeOfDay.Hours}:{DateTime.Now.TimeOfDay.Minutes}";
        return View();
    }

    public IActionResult LendBookToMember(string ISBN, string nationalCode,int p)
    {
        Dictionary<string, string> data = new()
        {
            {"ISBN",ISBN },
            {"nationalCode",nationalCode }
        };
        return View(data);
    }

    [HttpPost]
    public IActionResult LendBookToMember(string ISBN, string nationalCode)
    {
        var member = _memberRepository.FindMemberByNationalCode(nationalCode);
        var book = _bookRepository.FindBookByISBN(ISBN);
        if (member is not null && book is not null && book.IsAvailable == "موجود")
        {
            book.Member = member.NationalCode;
            book.IsAvailable = "ناموجود";
            BorrowedBookDetails details = new() { BorrowedBook = book, TakeDate = DateTime.Now };
            member.BorrowedBooksHistory.Add(details);
            _bookRepository.UpdateBook(book);
            _memberRepository.UpdateMember(member);
            ViewBag.lendingMessage = $"{book} با موفقیت به {member} واگذار شد";
            Dictionary<string, string> data = new()
            {
                {"ISBN","" },
                {"nationalCode",nationalCode }
            };
            return View(data);
        }
        else
        {
            if (member is null) ViewBag.wrongMember = "کد ملی وارد شده صحیح نیست";
            if (book is null) ViewBag.wrongBook = "شابک وارد شده صحیح نیست";
            if (book is not null && book.IsAvailable != "موجود") ViewBag.lendingMessage = $"کتاب {book} قبلا امانت داده شده است";
            Dictionary<string, string> data = new()
            {
                {"ISBN",ISBN },
                {"nationalCode",nationalCode }
            };
            return View(data);
        }
    }

    [HttpPost]
    public IActionResult LendBookToMemberFromProfile(int memberId ,string ISBN)
    {
        var member = _memberRepository.FindMemberById(memberId);
        var book = _bookRepository.FindBookByISBN(ISBN);
        if (book is not null && book.IsAvailable == "موجود")
        {
            book.Member = member.NationalCode;
            book.IsAvailable = "ناموجود";
            BorrowedBookDetails details = new() { BorrowedBook = book, TakeDate = DateTime.Now };
            member.BorrowedBooksHistory.Add(details);
            _bookRepository.UpdateBook(book);
            _memberRepository.UpdateMember(member);
            TempData["lendingMessage"] = $"{book} با موفقیت به عضو واگذار شد";
            return RedirectToRoute(new { controller = "Member", action = "MemberDetails", id = memberId });
        }
        else
        {
            if (book is null) TempData["lendingMessage"] = "شابک وارد شده صحیح نیست";
            if (book is not null && book.IsAvailable != "موجود") TempData["lendingMessage"] = $"کتاب {book} قبلا امانت داده شده است";
            return RedirectToRoute(new { controller = "Member", action = "MemberDetails", id = memberId });
        }
    }

    public IActionResult TakeBookFromMember(int bookId, int recordId, int memberId)
    {
        var member = _memberRepository.FindMemberById(memberId);
        var book = _bookRepository.FindBookById(bookId);
        var record = member.BorrowedBooksHistory.Find(b => b.Id == recordId);
        book.IsAvailable = "موجود";
        book.Member = "";
        record.Status = "تحویل شده";
        record.ReturnDate = DateTime.Now;
        _bookRepository.UpdateBook(book);
        _memberRepository.UpdateMember(member);
        TempData["lendingMessage"] = $"{book} از کاربر دریافت شد.";
        return RedirectToRoute(new { controller = "Member", action = "MemberDetails", id = memberId });
    }

    public string PersianCal()
    {
        PersianCalendar persianCalendar = new();
        int year = persianCalendar.GetYear(DateTime.Now);
        int month = persianCalendar.GetMonth(DateTime.Now);
        int day = persianCalendar.GetDayOfMonth(DateTime.Now);
        string strMonth = "";
        switch (month)
        {
            case 1:
                strMonth = "فروردین";
                break;
            case 2:
                strMonth = "اردیبهشت";
                break;
            case 3:
                strMonth = "خرداد";
                break;
            case 4:
                strMonth = "تیر";
                break;
            case 5:
                strMonth = "مرداد";
                break;
            case 6:
                strMonth = "شهریور";
                break;
            case 7:
                strMonth = "مهر";
                break;
            case 8:
                strMonth = "آبان";
                break;
            case 9:
                strMonth = "آذر";
                break;
            case 10:
                strMonth = "دی";
                break;
            case 11:
                strMonth = "بهمن";
                break;
            case 12:
                strMonth = "اسفند";
                break;
        }
        return  $"{day} {strMonth} {year}";
    }

}