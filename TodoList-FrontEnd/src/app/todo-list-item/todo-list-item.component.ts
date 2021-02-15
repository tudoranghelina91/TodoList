import { Component, Inject, OnInit } from '@angular/core';
import { TodoListItem } from '../interfaces/ITodoListItem';
import { ActivatedRoute, Router } from '@angular/router';
import { TodoListService } from '../services/todo-list.service';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-todo-list-item',
  templateUrl: './todo-list-item.component.html',
  styleUrls: ['./todo-list-item.component.css']
})
export class TodoListItemComponent implements OnInit {

  todoListItem : TodoListItem;
  todoListService : TodoListService;
  submitText : String;
  detailText : String;
  canDelete : Boolean;
  notLoaded : Boolean;

  public todoListItemFormGroup : FormGroup;

  constructor(private route : ActivatedRoute, httpClient : HttpClient, private router : Router, private _snackBar : MatSnackBar, private dialog : MatDialog) {
    this.todoListService = new TodoListService(httpClient);
    this.todoListItemFormGroup = new FormGroup({
      id : new FormControl(),
      name : new FormControl(),
      description : new FormControl(),
      completed : new FormControl(),
      todoListId : new FormControl()
    });
   }

  ngOnInit(): void {
    this.notLoaded = true;
    this.route.params.subscribe(params => {
      if (params['todoListItemId'] != null && this.route.snapshot.params['todoListId'] != null) {
        this.todoListService.getTodoListItem(params['todoListItemId'])
        .subscribe(data => {
            this.todoListItem = data;
            this.todoListItemFormGroup.get('todoListItemId').setValue(data.id);
            this.todoListItemFormGroup.get('name').setValue(data.name);
            this.todoListItemFormGroup.get('description').setValue(data.description);
            this.todoListItemFormGroup.get('completed').setValue(data.completed);
            this.todoListItemFormGroup.get('todoListId').setValue(data.todoListId);

            this.detailText = "Edit";
            this.canDelete = true;
            this.submitText = "Save Changes";
            this.notLoaded = false;
          }, error => {
            this.notLoaded = true;
          });
        } else {
          this.todoListItem = new TodoListItem();
          this.todoListItem.completed = false;
          this.todoListItem.todoListId = this.route.snapshot.params['todoListId'];
          this.todoListItemFormGroup.get('completed').setValue(false);
          this.submitText = "Add";
          this.detailText = "Add New Item";
          this.notLoaded = false;
      }
    }, error => {
      this.notLoaded = true;
    })
  }

  onSubmit() : void {
    let todoListItem = new TodoListItem()

    todoListItem.id = this.todoListItem.id;
    todoListItem.todoListId = this.todoListItem.todoListId;
    
    if (this.todoListItemFormGroup.get('name').value != null)
    {
      todoListItem.name = String(this.todoListItemFormGroup.get('name').value).trim();
      this.todoListItemFormGroup.get('name').setValue(null);
    }

    if (this.todoListItemFormGroup.get('description').value != null)
    {
      todoListItem.description = String(this.todoListItemFormGroup.get('description').value).trim();
      this.todoListItemFormGroup.get('description').setValue(null);
    }
    todoListItem.completed = this.todoListItemFormGroup.get('completed').value;

    if (todoListItem.name != null && todoListItem.name != "" &&  todoListItem.completed != null) {
      if (todoListItem.id != null) {
        this.todoListService.updateTodoListItem(todoListItem)
          .subscribe(data => {
            this.todoListItem.name = data.name;
            this.todoListItem.description = data.description;
            this.todoListItem.completed = data.completed;
            this.todoListItem.todoListId = data.todoListId;
            this.router.navigateByUrl('');
          }, error => {
            this._snackBar.open(error.message, null, { 'duration' : 5000 });
          });
      } else {
        this.todoListService.insertTodoListItem(todoListItem)
        .subscribe(data => {
          this.todoListItem.name = data.name;
          this.todoListItem.description = data.description;
          this.todoListItem.completed = data.completed;
          this.todoListItem.todoListId = data.todoListId;
          this.router.navigateByUrl('');
        }, error => {
          this._snackBar.open(error.message, null, { 'duration' : 5000 });
        });
      }
    } else {
      this._snackBar.open("Name is required", null, { 'duration' : 5000 });
      this.todoListItemFormGroup.get('name').setErrors({required : true})
    }
  }

  goBack() : void {
    this.router.navigateByUrl(`lists/`);
  }

  deleteTodoListItem() : void {
    let dialogRef = this.dialog.open(ConfirmationDialogComponent, 
      {
        width : '250px', 
        data : { 
          title: 'Are you sure?', 
          message : 'Do you want to delete ' + this.todoListItem.name + '?'
        }
      });
    dialogRef.afterClosed()
    .subscribe(result => {
      if (result === true) {
        this.todoListService.deleteTodoListItem(this.todoListItem.id)
        .subscribe(data => {
          this._snackBar.open(data.name + " has been deleted!", null, {'duration' : 5000});
          this.router.navigateByUrl('');
        }, error => {
          this._snackBar.open(error.message, null, {'duration' : 5000});
        });
      }
    });
  }

}