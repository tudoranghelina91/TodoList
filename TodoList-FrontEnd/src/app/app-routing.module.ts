import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TodoListItemComponent } from './todo-list-item/todo-list-item.component';
import { TodoListComponent } from './todo-list/todo-list.component';


const routes: Routes = [
  {path: '', component : TodoListComponent},
  {path: 'add', component: TodoListItemComponent},
  {path: ':id', component : TodoListItemComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
