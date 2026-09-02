namespace Workgrid.Models;

public class WorkTask : BaseEntity
{
   

    public long OrganizationId { get; set; }

    public long ProjectId { get; set; }

    public long? AssignedToUserId { get; set; }

    public long CreatedByUserId { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public string Status { get; set; }

    public string Priority { get; set; }

    public DateTime? DueDate { get; set; }

   
}