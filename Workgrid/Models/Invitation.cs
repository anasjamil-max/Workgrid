namespace Workgrid.Models;

public class Invitation : BaseEntity
{
    public long OrganizationId { get; set; }

    public long InvitedByUserId { get; set; }

    public string Email { get; set; }

    public string Role { get; set; }

    public string Token { get; set; }

    public string Status { get; set; }

    public DateTime ExpiresAt { get; set; }

}