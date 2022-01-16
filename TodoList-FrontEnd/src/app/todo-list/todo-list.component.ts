import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { TodoListItem } from '../interfaces/ITodoListItem';
import { TodoListService } from '../services/todo-list.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PageEvent } from '@angular/material/paginator';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { FormControl, FormGroup } from '@angular/forms';
import { TodoList } from '../interfaces/ITodoList';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.css']
})
export class TodoListComponent implements OnInit {
  todoList : TodoList;
  todoListItems : TodoListItem[];
  todoListService : TodoListService;
  lastUpdatedTodoListItem : TodoListItem;
  notLoaded : Boolean = true;
  success : Boolean;

  detailText : String;
  canDelete : Boolean;
  submitText : String;

  public todoListFormGroup : FormGroup;

  pageEvent: PageEvent;
  pageSizeOptions: number[] = [5, 10, 25, 50, 100];

  constructor(httpClient : HttpClient, private _snackBar : MatSnackBar, private route : ActivatedRoute, private router : Router, private dialog : MatDialog ) {
    this.todoListService = new TodoListService(httpClient);
    this.pageEvent = new PageEvent();
    
    this.todoListFormGroup = new FormGroup({
      id : new FormControl(),
      name : new FormControl(),
      description : new FormControl()
    });
   }

  ngOnInit(): void {
    this.todoListItems = new Array<TodoListItem>(0);
    this.todoList = new TodoList();
    this.notLoaded = true;
    this.route.params.subscribe(params => {
      if (params['todoListId'] != null) {
        this.todoListService.getTodoList(params['todoListId'])
        .subscribe(data => {
            this.todoList = data;
            if (this.todoList == null) {
              this.goBack();
            }
            this.todoListFormGroup.get('id').setValue(data.id);
            this.todoListFormGroup.get('name').setValue(data.name);
            this.todoListFormGroup.get('description').setValue(data.description);

            this.detailText = "Edit";
            this.canDelete = true;
            this.submitText = "Save Changes";
            this.notLoaded = false;

            this.todoListService.getTodoListItemsCount(this.todoList.id)
            .subscribe(length => this.pageEvent.length = length);
        
            this.pageEvent.pageSize = this.pageSizeOptions[0];
            this.pageEvent.pageIndex = 0;
            this.getServerData(this.pageEvent);
          }, error => {
            this.notLoaded = true;
            this.goBack();
          });
        } 
        else {
          this.todoList = new TodoList();
          this.submitText = "Add";
          this.detailText = "Add New Todo List";
          this.notLoaded = false;
      }
    }, error => {
      this.notLoaded = true;
      this.goBack();
    })
  }

  updateTodoListItemStatus(todoListItem : TodoListItem)
  {
    this.toggleTodoListItemStatus(todoListItem);
    this.todoListService.updateTodoListItem(todoListItem)
    .subscribe();
  }

  toggleTodoListItemStatus(todoListItem : TodoListItem) {
    if (!todoListItem.completed) {
      todoListItem.completed = true;
    } else {
      todoListItem.completed = false;
    }
  }

  getServerData(event?:PageEvent) {
    this.notLoaded = true;
    this.success = false;
    this.todoListService.getTodoListItems(this.todoList.id, event.pageIndex + 1, event.pageSize)
    .subscribe(data => 
      {
        this.todoListItems = data;
        this.notLoaded = false;
        this.success = true;
      }, error => {
        this.notLoaded = false;
        this.success = false;
        this._snackBar.open(error.message, null, {'duration' : 5000});
      });
      return event;
  }

  onSubmit() : void {
    let todoList = new TodoList()

    todoList.id = this.todoList.id;
    if (this.todoListFormGroup.get('name').value != null)
    {
      todoList.name = String(this.todoListFormGroup.get('name').value).trim();
      this.todoListFormGroup.get('name').setValue(null);
    }

    if (this.todoListFormGroup.get('description').value != null)
    {
      todoList.description = String(this.todoListFormGroup.get('description').value).trim();
      this.todoListFormGroup.get('description').setValue(null);
    }

    if (todoList.name != null && todoList.name != "") {
      if (todoList.id != null) {
        this.todoListService.updateTodoList(todoList)
          .subscribe(data => {
            this.todoList.name = data.name;
            this.todoList.description = data.description;
            this.goBack();
          }, error => {
            this._snackBar.open(error.message, null, { 'duration' : 5000 });
          });
      } else {
        this.todoListService.insertTodoList(todoList)
        .subscribe(data => {
          this.todoList.name = data.name;
          this.todoList.description = data.description;
          this.goBack();
        }, error => {
          this._snackBar.open(error.message, null, { 'duration' : 5000 });
        });
      }
    } else {
      this._snackBar.open("Name is required", null, { 'duration' : 5000 });
      this.todoListFormGroup.get('name').setErrors({required : true})
    }
  }

  deleteTodoList() : void {
    let dialogRef = this.dialog.open(ConfirmationDialogComponent, 
      {
        width : '250px', 
        data : { 
          title: 'Are you sure?', 
          message : 'Do you want to delete ' + this.todoList.name + '?'
        }
      });
    dialogRef.afterClosed()
    .subscribe(result => {
      if (result === true) {
        this.todoListService.deleteTodoList(this.todoList.id)
        .subscribe(data => {
          this._snackBar.open(data.name + " has been deleted!", null, {'duration' : 5000});
          this.goBack();
        }, error => {
          this._snackBar.open(error.message, null, {'duration' : 5000});
        });
      }
    });
  }

  goBack() : void {
    this.router.navigateByUrl('lists');
  }

}
