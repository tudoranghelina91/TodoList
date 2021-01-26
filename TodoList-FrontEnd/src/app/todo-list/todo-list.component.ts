import { HttpClient } from '@angular/common/http';
import { Component, OnInit, SimpleChanges } from '@angular/core';
import { TodoListItem } from '../interfaces/ITodoListItem';
import { TodoListService } from '../services/todo-list.service';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.css']
})
export class TodoListComponent implements OnInit {

  todoListItems : TodoListItem[];
  todoListService : TodoListService;
  lastUpdatedTodoListItem : TodoListItem;
  notLoaded : Boolean = true;
  success : Boolean;

  constructor(httpClient : HttpClient, private _snackBar : MatSnackBar) {
    this.todoListService = new TodoListService(httpClient)
   }

  ngOnInit(): void {
    this.todoListService.getTodoListItems()
    .subscribe(data => 
      {
        this.todoListItems = data;
        this.notLoaded = false;
        this.success = true;
      }, error => {
        this.notLoaded = false;
        this.success = false;
        this._snackBar.open(error.message, null, {'duration' : 5000});
      });
  }

  updateTodoListItemStatus(todoListItem : TodoListItem)
  {
    this.toggleTodoListItemStatus(todoListItem);
    this.todoListService.updateTodoListItem(todoListItem)
    .subscribe();
  }
  toggleTodoListItemStatus(todoListItem : TodoListItem) {
    if (!todoListItem.completed) {
      todoListItem.completed = true;
    } else {
      todoListItem.completed = false;
    }
  }

}
