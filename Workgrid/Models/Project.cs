namespace Workgrid.Models;

public class Project : BaseEntity
{
 

    public long OrganizationId { get; set; }

    public long TeamId { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public string Status { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? DueDate { get; set; }

    public long CreatedByUserId { get; set; }

    
}