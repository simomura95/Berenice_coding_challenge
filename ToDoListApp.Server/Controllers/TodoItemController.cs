using Microsoft.AspNetCore.Mvc;
using ToDoListApp.Server.Entities;
using ToDoListApp.Server.Entities.DTO;
using ToDoListApp.Server.Repositories.Interface;

namespace ToDoListApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemController : Controller
    {
        // I put all db operations in a repository; here I just call them
        private readonly IToDoItemRepository toDoItemRepository;

        public TodoItemController(IToDoItemRepository toDoItemRepository)
        {
            this.toDoItemRepository = toDoItemRepository;
        }


        // GET: fetch all existing todo items
        [HttpGet]
        public async Task<IActionResult> ReadTodoItems()
        {
            var todoItems = await toDoItemRepository.ReadAsync();  // call to repository and db

            var response = todoItems.Select(toDoItem => new ResponseTodoItemDto
            {
                Id = toDoItem.Id,
                Title = toDoItem.Title,
                Content = toDoItem.Content,
                CreatedAt = toDoItem.CreatedAt,
                UpdatedAt = toDoItem.UpdatedAt
            }).ToList();

            return Ok(response);
        }


        // POST: add a new todo item
        [HttpPost]
        public async Task<IActionResult> CreateTodoItem(CreateTodoItemDto request)
        {
            // Map DTO to domain model
            var toDoItem = new ToDoItem
            {
                // ID is handled by Entity Framework, UpdatedAt is equal to now by default
                Title = request.Title,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow
            };

            await toDoItemRepository.CreateAsync(toDoItem); // call to repository and db

            // Domain model to Dto
            var response = new ResponseTodoItemDto
            {
                Id = toDoItem.Id,
                Title = toDoItem.Title,
                Content = toDoItem.Content,
                CreatedAt = toDoItem.CreatedAt,
                UpdatedAt = toDoItem.UpdatedAt
            };

            return Ok(response);
        }


        // PATCH: edit a todo item
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTodoItem(UpdateTodoItemDto request, Guid id)
        {
            var toDoItem = new ToDoItem
            {
                Id = id,
                Title = request.Title,
                Content = request.Content,
                UpdatedAt = DateTime.UtcNow
            };

            // the update could fail if the item to edit is not found; if this happens, the following method returns null
            var updatedItem = await toDoItemRepository.UpdateAsync(toDoItem); // call to repository and db

            if (updatedItem is not null)
            {
                var response = updatedItem;

                return Ok(response);
            } else
            {
                return BadRequest("To-do item not found");
            }
        }


        // DELETE: remove a todo item
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {

            // the update could fail if the item to edit is not found; if this happens, the following method returns null
            var deletedItem = await toDoItemRepository.DeleteAsync(id); // call to repository and db

            if (deletedItem is not null)
            {
                var response = deletedItem;

                return Ok(response);
            } else
            {
                return BadRequest("To-do item not found");
            }
        }
    }
}
 