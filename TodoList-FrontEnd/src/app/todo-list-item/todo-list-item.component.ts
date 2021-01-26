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

  public todoListItemFormGroup : FormGroup;

  constructor(private route : ActivatedRoute, httpClient : HttpClient, private router : Router, private _snackBar : MatSnackBar, private dialog : MatDialog) {
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
      if (params['id'] != null) {
        this.todoListService.getTodoListItem(params['id'])
        .subscribe(data => {
            this.todoListItem = data;
            this.todoListItemFormGroup.get('id').setValue(data.id);
            this.todoListItemFormGroup.get('name').setValue(data.name);
            this.todoListItemFormGroup.get('description').setValue(data.description);
            this.todoListItemFormGroup.get('completed').setValue(data.completed);

            this.detailText = "Edit";
            this.canDelete = true;
            this.submitText = "Save Changes";
          });
        } 
        else {
          this.todoListItem = new TodoListItem();
          this.todoListItem.completed = false;
          this.todoListItemFormGroup.get('completed').setValue(false);
          this.submitText = "Add";
          this.detailText = "Add New Item";
      }
    })
  }

  onSubmit() : void {
    this.todoListItem.name = this.todoListItemFormGroup.get('name').value;
    this.todoListItem.description = this.todoListItemFormGroup.get('description').value;
    this.todoListItem.completed = this.todoListItemFormGroup.get('completed').value;
    if (this.todoListItem.name != null && this.todoListItem.description != null && this.todoListItem.completed != null) {
      if (this.todoListItem.id != null) {
        this.todoListService.updateTodoListItem(this.todoListItem)
          .subscribe(data => {
            this.todoListItem.name = data.name;
            this.todoListItem.description = data.description;
            this.todoListItem.completed = data.completed;
            this.router.navigateByUrl('');
          }, error => {
            this._snackBar.open(error.message, null, { 'duration' : 5000 });
          });
      } else {
        this.todoListService.insertTodoListItem(this.todoListItem)
        .subscribe(data => {
          this.todoListItem.name = data.name;
          this.todoListItem.description = data.description;
          this.todoListItem.completed = data.completed;
          this.router.navigateByUrl('');
        }, error => {
          this._snackBar.open(error.message, null, { 'duration' : 5000 });
        });
      }
    } else {
      this._snackBar.open("Please check the required fields", null, { 'duration' : 5000 });
    }
  }

  goBack() : void {
    this.router.navigateByUrl('');
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