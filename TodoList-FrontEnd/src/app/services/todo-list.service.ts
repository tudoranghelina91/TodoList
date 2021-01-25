import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { TodoListItem } from '../interfaces/ITodoListItem';

@Injectable({
  providedIn: 'root'
})
export class TodoListService {

  constructor(private httpClient : HttpClient) { }
  getTodoListItems()
  {
    return this.httpClient.get<TodoListItem[]>('https://localhost:44307/api/todolistitems');
  }
}
