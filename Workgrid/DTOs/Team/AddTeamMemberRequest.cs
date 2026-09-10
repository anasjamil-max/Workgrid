namespace Workgrid.DTOs.Team;

public class AddTeamMemberRequest
{

    public long TeamId { get; set; }
    public long UserId { get; set; }
    public string Role { get; set; }
}