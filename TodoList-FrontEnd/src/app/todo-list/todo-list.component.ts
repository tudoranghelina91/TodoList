import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { TodoListItem } from '../interfaces/ITodoListItem';
import { TodoListService } from '../services/todo-list.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PageEvent } from '@angular/material/paginator';

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

  pageEvent: PageEvent;
  pageSizeOptions: number[] = [5, 10, 25, 50, 100];

  constructor(httpClient : HttpClient, private _snackBar : MatSnackBar) {
    this.todoListService = new TodoListService(httpClient);
    this.pageEvent = new PageEvent();
   }

  ngOnInit(): void {
    this.todoListService.getTodoListItemsCount()
    .subscribe(length => this.pageEvent.length = length);

    this.pageEvent.pageSize = this.pageSizeOptions[0];
    this.pageEvent.pageIndex = 0;
    this.getServerData(this.pageEvent);
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

  getServerData(event?:PageEvent) {
    this.notLoaded = true;
    this.success = false;
    this.todoListService.getTodoListItems(event.pageIndex + 1, event.pageSize)
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

      return event;
  }

}
