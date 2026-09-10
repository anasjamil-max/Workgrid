namespace Workgrid.DTOs.Organization;

public class InviteUserRequest
{
    public long OrganizationId { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}