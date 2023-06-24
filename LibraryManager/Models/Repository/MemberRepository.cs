using LibraryManager.Models.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Models.Repository;

public class MemberRepository : IMemberRepository
{
    private readonly Contexts _context;

    public MemberRepository(Contexts context)
    {
        _context = context;
    }

    //Create
    public void CreateMember(Member member)
    {
        _context.Members.Add(member);
        _context.SaveChanges();
    }
    //Read
    public Member? FindMemberById(int id)
    {
        return _context.Members.Include(m => m.BorrowedBooksHistory).ThenInclude(b => b.BorrowedBook).FirstOrDefault(m => m.Id == id);
    }
    public Member? FindMemberByNationalCode(string nationalCode)
    {
        return _context.Members.ToList().Find(member => member.NationalCode == nationalCode);
    }
    public List<Member> ReadAllMembers()
    {
        return _context.Members.ToList();
    }
    public List<Member> SearchMembers(string firstName, string lastName, string nationalCode)
    {
        return _context.Members
            .Where(member => member.FirstName.ToUpper().Contains(firstName) &&
                             member.LastName.ToUpper().Contains(lastName) &&
                             member.NationalCode.Contains(nationalCode)).ToList();
    }

    //Update
    public void UpdateMember(Member member)
    {
        _context.Entry(member).State = EntityState.Modified;
        _context.SaveChanges();
    }
    //Delete
    public void DeleteMember(int id)
    {
        Member deletedMember = _context.Members.Include(m => m.BorrowedBooksHistory).FirstOrDefault(m => m.Id == id);
        _context.Members.Remove(deletedMember);
        _context.SaveChanges();

    }
}
