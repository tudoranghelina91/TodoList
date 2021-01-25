import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TodoListItem } from '../interfaces/ITodoListItem';
import { TodoListService } from '../services/todo-list.service';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.css']
})
export class TodoListComponent implements OnInit {

  todoListItems : TodoListItem[];
  todoListService : TodoListService;
  constructor(httpClient : HttpClient) {
    this.todoListService = new TodoListService(httpClient)
   }

  ngOnInit(): void {
    this.todoListService.getTodoListItems()
    .subscribe(data => 
      {
        this.todoListItems = data;
      }, error => {

      });
  }

  editTodoListItem(todoListItem : TodoListItem)
  {
    
  }

}
