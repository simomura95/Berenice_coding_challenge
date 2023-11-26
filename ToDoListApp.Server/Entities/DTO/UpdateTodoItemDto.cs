namespace ToDoListApp.Server.Entities.DTO
{
  public class UpdateTodoItemDto
  {
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  }
}