import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { TodoListItem } from '../interfaces/ITodoListItem';

@Injectable({
  providedIn: 'root'
})
export class TodoListService {

  constructor(private httpClient : HttpClient) { }

  baseUri = "https://dostuff.azurewebsites.net/api/todolistitems";

  getTodoListItems(page : number, count : number)
  {
    return this.httpClient.get<TodoListItem[]>(`${this.baseUri}/${page}/${count}`);
  }

  getTodoListItemsCount()
  {
    return this.httpClient.get<number>(`${this.baseUri}/GetItemsCount`);
  }

  getTodoListItem(id : number)
  {
    return this.httpClient.get<TodoListItem>(`${this.baseUri}/${id}`);
  }

  updateTodoListItem(todoListItem : TodoListItem)
  {
      return this.httpClient.put<TodoListItem>(`${this.baseUri}`, todoListItem);
  }

  insertTodoListItem(todoListItem : TodoListItem) {
    return this.httpClient.post<TodoListItem>(`${this.baseUri}`, todoListItem);
  }

  deleteTodoListItem(id : number) {
    return this.httpClient.delete<TodoListItem>(`${this.baseUri}/${id}`);
  }
}