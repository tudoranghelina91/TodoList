import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TodoList } from '../interfaces/ITodoList';

import { TodoListItem } from '../interfaces/ITodoListItem';

@Injectable({
  providedIn: 'root'
})
export class TodoListService {

  constructor(private httpClient : HttpClient) { }

  baseUri = "https://apidostuff.azurewebsites.net/api";
  listUri = "todolists";
  itemsUri = "todolistitems"

  getTodoLists(page : number, count : number) {
    return this.httpClient.get<TodoList[]>(`${this.baseUri}/${this.listUri}/${page}/${count}`);
  }

  getTodoListsCount()
  {
    return this.httpClient.get<number>(`${this.baseUri}/${this.listUri}/GetItemsCount/`);
  }

  getTodoList(id : number)
  {
    return this.httpClient.get<TodoList>(`${this.baseUri}/${this.listUri}/${id}`);
  }

  updateTodoList(todoList : TodoList)
  {
      return this.httpClient.put<TodoList>(`${this.baseUri}/${this.listUri}`, todoList);
  }

  insertTodoList(todoList : TodoList) {
    return this.httpClient.post<TodoList>(`${this.baseUri}/${this.listUri}`, todoList);
  }

  deleteTodoList(id : number) {
    return this.httpClient.delete<TodoList>(`${this.baseUri}/${this.listUri}/${id}`);
  }

  getTodoListItems(list : number, page : number, count : number)
  {
    return this.httpClient.get<TodoListItem[]>(`${this.baseUri}/${this.itemsUri}/${list}/${page}/${count}`);
  }

  getTodoListItemsCount(list : number)
  {
    return this.httpClient.get<number>(`${this.baseUri}/${this.itemsUri}/GetItemsCount/${list}`);
  }

  getTodoListItem(id : number)
  {
    return this.httpClient.get<TodoListItem>(`${this.baseUri}/${this.itemsUri}/${id}`);
  }

  updateTodoListItem(todoListItem : TodoListItem)
  {
      return this.httpClient.put<TodoListItem>(`${this.baseUri}/${this.itemsUri}`, todoListItem);
  }

  insertTodoListItem(todoListItem : TodoListItem) {
    return this.httpClient.post<TodoListItem>(`${this.baseUri}/${this.itemsUri}`, todoListItem);
  }

  deleteTodoListItem(id : number) {
    return this.httpClient.delete<TodoListItem>(`${this.baseUri}/${this.itemsUri}/${id}`);
  }
}