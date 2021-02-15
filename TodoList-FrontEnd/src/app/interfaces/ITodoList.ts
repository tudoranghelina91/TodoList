import { TodoListItem } from "./ITodoListItem";

export interface TodoList
{
    id : number;
    name : String;
    description : String;
    todoListItems : TodoListItem[];
}

export class TodoList
{
    id : number;
    name : String;
    description : String;
    todoListItems : TodoListItem[];
}