import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';

export interface TodoItem {
  id: string;
  title: string;
  content: string;
  createdAt: Date;
  updatedAt: Date;
  isEditing: boolean;
}

export interface NewTodoItem {
  title: string;
  content: string;
}

@Injectable({
  providedIn: 'root'
})
export class TodoService {
  // todoItems is an observable, so it can stay here on the service and send any update to all components subscribed to it (just item-list, since add-item has no need to read it)
  private todoItemsSubject = new BehaviorSubject<TodoItem[]>([]);
  todoItems$: Observable<TodoItem[]> = this.todoItemsSubject.asObservable();

  constructor(private http: HttpClient) {}

  toggleEditMode(todoItem: TodoItem) {
    todoItem.isEditing = !todoItem.isEditing;
  }

  // get todo items from db (executed on start-up)
  // subscribe is needed and similar to async: when the function gets an answer, it updates the todo list (or rather its value, as it's an observable)
  getTodoItems() {
    this.http.get<TodoItem[]>('/api/todoitem').subscribe(
      (result) => {
        // this.todoItems = result;
        this.todoItemsSubject.next(result);
      },
      (error) => {
        console.error(error);
      }
    );
  }

  // add a new todo item
  addTodoItem(newTodoItem: NewTodoItem) {
    this.http.post<TodoItem>('/api/todoitem', newTodoItem).subscribe(
      (result) => {
        this.todoItemsSubject.next([...this.todoItemsSubject.value, result]);
      },
      (error) => {
        console.error(error);
      }
    );
  }

  // remove a todo item
  removeTodoItem(todoItem: TodoItem) {
    this.http.delete<TodoItem>(`/api/todoitem/${todoItem.id}`).subscribe(
      (result) => {
        const updatedItems = this.todoItemsSubject.value.filter(todoItem => todoItem.id !== result.id);
        this.todoItemsSubject.next(updatedItems);
      },
      (error) => {
        console.error(error);
      }
    );
  }

  // update a todo item (change its title or content)
  editTodoItem(todoItem: TodoItem, editedItem: NewTodoItem) {
    this.http.patch<TodoItem>(`/api/todoitem/${todoItem.id}`, editedItem).subscribe(
      (result) => {
        // todoItem = result;  // NO: this way, todoItem inside todoItems is not updated, and is not re-rendered in the page; the content is still editable and the button still reads "save"
        const updatedItems = this.todoItemsSubject.value.map(todoItem => todoItem.id === result.id ? result : todoItem );
        this.todoItemsSubject.next(updatedItems);
        // or with findIndex instead of map; findIndex should be a little faster, but I think that map makes the code clearer, and with few elements the time difference is neglectable
        // this.toggleEditMode(todoItem) // not needed, as it becomes null
      },
      (error) => {
        console.error(error);
      }
    );
  }
}
