namespace Workgrid.Models;

public class TeamMember
{
    public long Id { get; set; }

    public long TeamId { get; set; }

    public long UserId { get; set; }

    public string Role { get; set; }

    public DateTime JoinedAt { get; set; }
}