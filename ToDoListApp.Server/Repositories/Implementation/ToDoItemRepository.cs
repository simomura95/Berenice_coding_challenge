using Microsoft.EntityFrameworkCore;
using ToDoListApp.Server.DbContext;
using ToDoListApp.Server.Entities;
using ToDoListApp.Server.Repositories.Interface;

namespace ToDoListApp.Server.Repositories.Implementation
{
    public class ToDoItemRepository : IToDoItemRepository
    {
        private readonly ToDoListAppDbContext dbContext;

        public ToDoItemRepository(ToDoListAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        

        // add a new item
        public async Task<ToDoItem> CreateAsync(ToDoItem toDoItem)
        {
            await dbContext.ToDoItems.AddAsync(toDoItem);
            await dbContext.SaveChangesAsync();

            return toDoItem;
        }


        // get all items
        public async Task<List<ToDoItem>> ReadAsync()
        {
            return await dbContext.ToDoItems.OrderBy(item => item.CreatedAt).ToListAsync();  // order by creation date, so edited elements stay in their place
        }


        // edit and update an item
        public async Task<ToDoItem?> UpdateAsync(ToDoItem toDoItem)
        {
            var itemToEdit = await dbContext.ToDoItems.FindAsync(toDoItem.Id);
            // it should never happen that an item is not found
            if (itemToEdit is not null)
            {
                itemToEdit.Title = toDoItem.Title;
                itemToEdit.Content = toDoItem.Content;
                itemToEdit.UpdatedAt = toDoItem.UpdatedAt;
                await dbContext.SaveChangesAsync();
            }

            return itemToEdit;  // can be a todo item or null (handled in the controller)
        }


        // delete an item
        public async Task<ToDoItem?> DeleteAsync(Guid itemId)
        {
            var itemToDelete = await dbContext.ToDoItems.FindAsync(itemId);
            // it should never happen that an item is not found
            if (itemToDelete is not null)
            {
                // no await required for Remove
                dbContext.ToDoItems.Remove(itemToDelete);
                await dbContext.SaveChangesAsync();
            }

            return itemToDelete; // can be a todo item or null (handled in the controller)
        }
    }
}