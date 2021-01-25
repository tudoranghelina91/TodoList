import { Component, OnInit } from '@angular/core';
import { TodoListItem } from '../interfaces/ITodoListItem';
import { ActivatedRoute, Router } from '@angular/router';
import { TodoListService } from '../services/todo-list.service';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-todo-list-item',
  templateUrl: './todo-list-item.component.html',
  styleUrls: ['./todo-list-item.component.css']
})
export class TodoListItemComponent implements OnInit {

  todoListItem : TodoListItem;
  todoListService : TodoListService;

  public todoListItemFormGroup : FormGroup;

  constructor(private route : ActivatedRoute, httpClient : HttpClient, private router : Router) {
    this.todoListService = new TodoListService(httpClient);
    this.todoListItemFormGroup = new FormGroup({
      id : new FormControl(),
      name : new FormControl(),
      description : new FormControl(),
      completed : new FormControl()
    });
   }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.todoListService.getTodoListItem(params['id'])
      .subscribe(data => {
        this.todoListItem = data;
        this.todoListItemFormGroup.get('id').setValue(data.id);
        this.todoListItemFormGroup.get('name').setValue(data.name);
        this.todoListItemFormGroup.get('description').setValue(data.description);
        this.todoListItemFormGroup.get('completed').setValue(data.completed);
      });
    })
  }

  onSubmit() : void {
    this.todoListItem.name = this.todoListItemFormGroup.get('name').value;
    this.todoListItem.description = this.todoListItemFormGroup.get('description').value;
    this.todoListItem.completed = this.todoListItemFormGroup.get('completed').value;
    this.todoListService.updateTodoListItem(this.todoListItem)
      .subscribe(data => {
        this.todoListItem.name = data.name;
        this.todoListItem.description = data.description;
        this.todoListItem.completed = data.completed;
        this.router.navigateByUrl('');
      }, error => {
        console.log("Something went wrong");
      });
  }

}
