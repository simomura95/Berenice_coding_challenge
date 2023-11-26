import { Component, OnInit } from '@angular/core';
import { TodoService, TodoItem, NewTodoItem } from '../service/todo.service';

@Component({
  selector: 'app-item-list',
  templateUrl: './item-list.component.html',
  styleUrls: ['./item-list.component.css']
})
export class ItemListComponent implements OnInit{
  public todoItems: TodoItem[] = []

  constructor(private todoService: TodoService) {}
  
  // on init, update item list in service and subscribe to it (so the view is updated when todoItems changes in the service)
  ngOnInit() {
    this.todoService.getTodoItems();
    this.todoService.todoItems$.subscribe(updatedItems => {
      this.todoItems = updatedItems;
    });
  }

  // remove an item (simple call to service)
  removeTodoItem(todoItem: TodoItem) {
    this.todoService.removeTodoItem(todoItem);
  }

  // edit an item (tracks changes and send them to the service)
  editTodoItem(todoItem: TodoItem) {
    const newTitle = document.getElementById(`title-${todoItem.id}`)!.textContent;
    const newContent = document.getElementById(`content-${todoItem.id}`)!.textContent
    const editedItem: NewTodoItem = {  // an empty field is accepted and null is treated as an empty string
      title: newTitle ? newTitle : '',
      content: newContent ? newContent : ''
    }
    this.todoService.editTodoItem(todoItem, editedItem)
  }

  // in edit mode, the item is editable and the button reads "save" instead of "edit"
  toggleEditMode(todoItem: TodoItem) {
    this.todoService.toggleEditMode(todoItem)
  }
}
