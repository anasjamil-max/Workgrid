namespace Workgrid.Models;

public class Team : BaseEntity
{
   
    public long OrganizationId { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    
}