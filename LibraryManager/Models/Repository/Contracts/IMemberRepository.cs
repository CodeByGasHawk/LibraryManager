namespace LibraryManager.Models.Repository.Contracts;

public interface IMemberRepository
{
    public void CreateMember(Member member);
    public Member FindMemberById(int id);
    public Member? FindMemberByNationalCode(string nationalCode);
    public List<Member> ReadAllMembers();
    public List<Member> SearchMembers(string firstName, string lastName, string nationalCode);
    public void UpdateMember(Member member);
    public void DeleteMember(int id);
}
