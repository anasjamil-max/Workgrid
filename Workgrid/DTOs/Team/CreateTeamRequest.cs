namespace Workgrid.DTOs.Team;

public class CreateTeamRequest
{
    public long OrganizationId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}