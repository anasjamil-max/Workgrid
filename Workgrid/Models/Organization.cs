namespace Workgrid.Models;

public class Organization : BaseEntity
{

    public string Name { get; set; }
    public string Slug { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    
}