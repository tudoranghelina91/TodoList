import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TodoListItemComponent } from './todo-list-item/todo-list-item.component';
import { TodoListComponent } from './todo-list/todo-list.component';
import { TodoListsComponent } from './todo-lists/todo-lists.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'lists',
    pathMatch: 'full'
  },
  {
    path: 'lists',
    component: TodoListsComponent,
  },
  {
    path: 'lists/add',
    component: TodoListComponent,
  },
  {
    path: 'lists/:todoListId',
    component: TodoListComponent,
    pathMatch: 'full',
  },
  {
    path: 'lists/:todoListId/add',
    component: TodoListItemComponent,
    pathMatch: 'full',
  },
  {
    path: 'lists/:todoListId/:todoListItemId',
    component: TodoListItemComponent,
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: 'lists',
    pathMatch: 'full'
  }
];


@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
