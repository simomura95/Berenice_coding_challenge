import { Component } from '@angular/core';
import { TodoService, NewTodoItem } from '../service/todo.service';

@Component({
  selector: 'app-add-todo-item',
  templateUrl: './add-todo-item.component.html',
  styleUrls: ['./add-todo-item.component.css']
})
export class AddTodoItemComponent {
  constructor(private todoService: TodoService) {}

    // add a new todo item
    newTodoItem: NewTodoItem = {
      title: '',
      content: ''
    }
    
    addTodoItem() {
      this.todoService.addTodoItem(this.newTodoItem);
      this.newTodoItem = {  // after adding the item, clear the form fields
        title: '',
        content: ''
      };
    }
}
