import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { TodoListItem } from '../interfaces/ITodoListItem';

@Injectable({
  providedIn: 'root'
})
export class TodoListService {

  constructor(private httpClient : HttpClient) { }

  getTodoListItems(page : number, count : number)
  {
    return this.httpClient.get<TodoListItem[]>('https://localhost:44307/api/todolistitems/' + page + "/" + count);
  }

  getTodoListItemsCount()
  {
    return this.httpClient.get<number>('https://localhost:44307/api/todolistitems/GetItemsCount');
  }

  getTodoListItem(id : number)
  {
    return this.httpClient.get<TodoListItem>('https://localhost:44307/api/todolistitems/' + id);
  }

  updateTodoListItem(todoListItem : TodoListItem)
  {
      return this.httpClient.put<TodoListItem>('https://localhost:44307/api/todolistitems', todoListItem);
  }

  insertTodoListItem(todoListItem : TodoListItem) {
    return this.httpClient.post<TodoListItem>('https://localhost:44307/api/todolistitems', todoListItem);
  }

  deleteTodoListItem(id : number) {
    return this.httpClient.delete<TodoListItem>('https://localhost:44307/api/todolistitems/' + id);
  }
}
