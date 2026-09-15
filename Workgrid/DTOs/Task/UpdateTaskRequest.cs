namespace Workgrid.DTOs.Task;

public class UpdateTaskRequest
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public long? AssignedToUserId { get; set; }
    public DateTime? DueDate { get; set; }
}