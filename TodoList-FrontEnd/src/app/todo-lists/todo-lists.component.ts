import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TodoList } from '../interfaces/ITodoList';
import { TodoListItem } from '../interfaces/ITodoListItem';
import { User } from '../interfaces/IUser';
import { LoginServiceService } from '../services/login-service.service';
import { TodoListService } from '../services/todo-list.service';

@Component({
  selector: 'app-todo-lists',
  templateUrl: './todo-lists.component.html',
  styleUrls: ['./todo-lists.component.css']
})
export class TodoListsComponent implements OnInit {

  constructor(httpClient : HttpClient, private _snackBar : MatSnackBar) {
    this.loginService = new LoginServiceService(httpClient, null);
    this.todoListService = new TodoListService(httpClient);
    this.pageEvent = new PageEvent();
   }

   ngOnInit(): void {
    this.todoListService.getTodoListsCount()
    .subscribe(length => this.pageEvent.length = length);

    this.pageEvent.pageSize = this.pageSizeOptions[0];
    this.pageEvent.pageIndex = 0;
    this.getServerData(this.pageEvent);
  }

  todoLists : TodoList[];
  todoListService : TodoListService;
  loginService : LoginServiceService;
  lastUpdatedTodoListItem : TodoListItem;
  notLoaded : Boolean = true;
  success : Boolean;
  user : User;

  pageEvent: PageEvent;
  pageSizeOptions: number[] = [5, 10, 25, 50, 100];

  getServerData(event?:PageEvent) {
    this.notLoaded = true;
    this.success = false;
    this.user = this.loginService.getUserDetails();
    this.todoListService.getTodoLists(event.pageIndex + 1, event.pageSize)
    .subscribe(data => 
      {
        this.todoLists = data;
        this.notLoaded = false;
        this.success = true;
      }, error => {
        this.notLoaded = false;
        this.success = false;
        this._snackBar.open(error.message, null, {'duration' : 5000});
      });

      return event;
  }

}
