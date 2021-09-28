import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginActivate } from './login-activate/login-activate.component';
import { LoginComponent } from './login/login.component';
import { TodoListItemComponent } from './todo-list-item/todo-list-item.component';
import { TodoListComponent } from './todo-list/todo-list.component';
import { TodoListsComponent } from './todo-lists/todo-lists.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'lists',
    component: TodoListsComponent,
    canActivate: [
      LoginActivate
    ]
  },
  {
    path: 'lists/add',
    component: TodoListComponent,
    canActivate: [
      LoginActivate
    ]
  },
  {
    path: 'lists/:todoListId',
    component: TodoListComponent,
    pathMatch: 'full',
    canActivate: [
      LoginActivate
    ]
  },
  {
    path: 'lists/:todoListId/add',
    component: TodoListItemComponent,
    pathMatch: 'full',
    canActivate: [
      LoginActivate
    ]
  },
  {
    path: 'lists/:todoListId/:todoListItemId',
    component: TodoListItemComponent,
    pathMatch: 'full',
    canActivate: [
      LoginActivate
    ]
  },
  {
    path: '**',
    redirectTo: 'lists',
    pathMatch: 'full',
    canActivate: [
      LoginActivate
    ]
  }
];


@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule],
  providers: [LoginActivate]
})
export class AppRoutingModule { }
