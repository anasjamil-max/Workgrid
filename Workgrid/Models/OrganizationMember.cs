namespace Workgrid.Models;

public class OrganizationMember
{
    public long Id { get; set; }
    public long OrganizationId { get; set; }

    public long UserId { get; set; }
    public string Role {  get; set; }
    public DateTime JoinedAt { get; set; }
    public bool IsActive { get; set; }


}