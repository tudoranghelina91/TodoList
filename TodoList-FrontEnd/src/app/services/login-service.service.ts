import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../interfaces/IUser';

@Injectable({
  providedIn: 'root'
})
export class LoginServiceService {

  constructor(private httpClient : HttpClient, private router : Router) { }
  
  baseUri = "https://apidostuff.azurewebsites.net/api";
  usersUri = "users";
  registerUri = "register";
  loginUri = "login";
  detailsUri = "details";
  loginStatusUri = "loginStatus";
  check = "loginStatus";
  
  register(user : User)
  {
    this.httpClient.post(`${this.baseUri}/${this.usersUri}/${this.registerUri}`, user).subscribe(() => { }, error => { }, () => {
      this.login(user);
    });
  }

  login(user : User)
  {
    this.httpClient.post<User>(`${this.baseUri}/${this.usersUri}/${this.loginUri}`, user)
    .subscribe(data => {
      localStorage.setItem("accessToken", data.accessToken);
      this.router.navigateByUrl('./lists');
    });
  }

  getUserDetails() : User
  {
    let user : User = new User();
    this.httpClient.get<User>(`${this.baseUri}/${this.usersUri}/${this.detailsUri}`)
      .subscribe(data => user.email = data.email);
    
    return user;
  }

  getLoginStatus() : boolean
  {
    return localStorage.getItem("accessToken") != null;
  }
}
