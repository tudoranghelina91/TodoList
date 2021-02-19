import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { TodoList } from './interfaces/ITodoList';
import { TodoListService } from './services/todo-list.service';

@Injectable({
  providedIn: 'root'
})
export class PreventUndefinedNullGuard implements CanActivate {
  constructor (private todoListService : TodoListService) {
  }

  private todoList : TodoList;
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      this.todoListService.getTodoList(route.params['todoListId']).subscribe(data => this.todoList = data);
      if (this.todoList == null) {
        return false;
      }
      return true;
  }
  
}
