using ToDoListApp.Server.Entities;

namespace ToDoListApp.Server.Repositories.Interface
{
  public interface IToDoItemRepository
  {
    Task<ToDoItem> CreateAsync(ToDoItem toDoItem);
    Task<List<ToDoItem>> ReadAsync();
    Task<ToDoItem?> UpdateAsync(ToDoItem toDoItem);
    Task<ToDoItem?> DeleteAsync(Guid itemId);
  }  
}