import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../interfaces/IUser';

@Injectable({
  providedIn: 'root'
})
export class LoginServiceService {

  constructor(private httpClient : HttpClient, private router : Router) { }
  
  baseUri = "https://localhost:5001/api";
  usersUri = "users";
  registerUri = "register";
  loginUri = "login";
  detailsUri = "details";
  loginStatusUri = "loginStatus";
  
  register(user : User)
  {
    this.httpClient.post<User>(`${this.baseUri}/${this.usersUri}/${this.registerUri}`, user).subscribe();
  }

  login(user : User)
  {
    this.httpClient.post<User>(`${this.baseUri}/${this.usersUri}/${this.loginUri}`, user)
    .subscribe(data => {
      
      if (data != null) {
        user.id = data.id;
        user.accessToken = data.accessToken;
        user.expiresIn = data.expiresIn;
        user.username = data.username;
        user.email = data.email;
        
        if (user.accessToken != null) {
          localStorage.setItem("accessToken", user.accessToken);
          localStorage.setItem("expires", user.expiresIn.toString());

          this.router.navigateByUrl('./lists');
        }
      }
    });
  }

  getUserDetails() : User
  {
    let user : User = new User();
    this.httpClient.get<User>(`${this.baseUri}/${this.usersUri}/${this.detailsUri}`)
      .subscribe(data => {
        user.email = data.email;
    });
    
    return user;
  }

  getLoginStatus() : boolean
  {
    return localStorage.getItem("accessToken") != null;
  }
}
