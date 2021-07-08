import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../interfaces/IUser';

@Injectable({
  providedIn: 'root'
})
export class LoginServiceService {

  constructor(private httpClient : HttpClient) { }
  
  baseUri = "https://localhost:44307/api";
  usersUri = "users";
  registerUri = "register";
  loginUri = "login";
  
  register(user : User)
  {
    this.httpClient.post<User>(`${this.baseUri}/${this.usersUri}/${this.registerUri}`, user).subscribe();
  }

  login(user : User)
  {
    this.httpClient.post<User>(`${this.baseUri}/${this.usersUri}/${this.loginUri}`, user).subscribe(data => {

      if (data != null) {
        user.accessToken = data.accessToken;
        user.expiresIn = data.expiresIn;
  
        localStorage.setItem("accessToken", user.accessToken);
        localStorage.setItem("expires", user.expiresIn.toString()); 
      }
    });
  }
}
