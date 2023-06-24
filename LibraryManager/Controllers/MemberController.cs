using LibraryManager.Models;
using LibraryManager.Models.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.Controllers;

public class MemberController : Controller
{
    private IMemberRepository _memberRepository;

    public MemberController(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public IActionResult AddMember()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddMember(Member member)
    {
        var existingMember = _memberRepository.FindMemberByNationalCode(member.NationalCode);
        if (ModelState.IsValid && existingMember is not null)
        {
            ViewBag.AddMemberMessage = $"کد ملی {member.NationalCode} قبلا به نام {existingMember} ثبت شده است.";
            return View();
        }
        else if (ModelState.IsValid && existingMember is null)
        {
            Normalizer.NormalizeMember(member);
            _memberRepository.CreateMember(member);
            string memberFullName = $"{member.FirstName} {member.LastName}";
            ViewBag.AddMemberMessage = $"عضو جدید \"{memberFullName}\" با موفقیت ثبت شد.";
            return View();
        }
        else
        {
            return View();
        }
    }

    public IActionResult PrintMembers()
    {
        var listOfMembers = _memberRepository.ReadAllMembers().OrderBy(member => member.LastName).ToList();
        return View(listOfMembers);
    }

    [HttpPost]
    public IActionResult PrintMembers(string firstName, string lastName, string nationalCode)
    {
        var searchResult = _memberRepository.SearchMembers(firstName ?? "".ToUpper(), lastName ?? "".ToUpper(), nationalCode ?? "")
            .OrderBy(member => member.LastName).ToList();
        return View(searchResult);
    }

    public IActionResult UpdateMember(int id)
    {
        var updatedMember = _memberRepository.FindMemberById(id);
        TempData["memberId"] = updatedMember.Id;
        return View(updatedMember);
    }

    [HttpPost]
    public IActionResult UpdateMember(Member member, int id)
    {
        var existingMember = _memberRepository.FindMemberByNationalCode(member.NationalCode);
        bool updateCondition = (existingMember is not null && existingMember.Id == id) || existingMember is null ? true : false;

        if (ModelState.IsValid && updateCondition)
        {
            Normalizer.NormalizeMember(member);
            var updatedMember = _memberRepository.FindMemberById(id);
            updatedMember.FirstName = member.FirstName;
            updatedMember.LastName = member.LastName;
            updatedMember.NationalCode = member.NationalCode;
            var borrowedBooks = updatedMember.BorrowedBooksHistory.Where(b => b.Status=="تحویل نشده");
            foreach(var record in borrowedBooks)
            {
                record.BorrowedBook.Member = member.NationalCode;
            }
            _memberRepository.UpdateMember(updatedMember);
            TempData["UpdateMemberMessage"] = $"تغییرات عضو \"{member}\" با موفقیت ثبت شد.";
            return RedirectToAction("MemberDetails", new {id = id});
        }
        else if (ModelState.IsValid && !updateCondition)
        {
            ViewBag.UpdateMemberMessage = $"کد ملی \"{member.NationalCode}\" قبلا به نام \"{existingMember}\" ثبت شده است.";
            member.BorrowedBooksHistory = _memberRepository.FindMemberById(id).BorrowedBooksHistory;
            return View(member);
        }
        else
        {
            return View(member);
        }
    }

    public IActionResult DeleteMember(int id)
    {
        var member = _memberRepository.FindMemberById(id);
        bool hasBook = member.BorrowedBooksHistory.Any(book => book.Status == "تحویل نشده");
        if (!hasBook)
        {            
            _memberRepository.DeleteMember(id);
            TempData["UpdateMemberMessage"] = $"{member} با موفقیت حذف شد.";
            return RedirectToAction("PrintMembers");
        }
        else
        {
            TempData["UpdateMemberMessage"] = $"کاربر کتاب تحویل نشده دارد";
            return RedirectToAction("PrintMembers");
        }
    }

    public IActionResult MemberDetails(int id)
    {
        var member = _memberRepository.FindMemberById(id);
        return View(member);
    }

    public IActionResult NationalCodeToId(string nationalCode)
    {
        var member = _memberRepository.FindMemberByNationalCode(nationalCode);
        int id = member.Id;
        return RedirectToAction("MemberDetails",new { id = id });
    } 
}

