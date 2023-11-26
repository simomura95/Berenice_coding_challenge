namespace ToDoListApp.Server.Entities.DTO
{
  public class CreateTodoItemDto
  {
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  }
}