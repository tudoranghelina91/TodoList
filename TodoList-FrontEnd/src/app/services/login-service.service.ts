import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import jwtDecode from 'jwt-decode';
import { User } from '../interfaces/IUser';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LoginServiceService {

  constructor(private httpClient : HttpClient, private router : Router) { }
  
  baseUri = environment.baseUri;
  usersUri = "users";
  registerUri = "register";
  loginUri = "login";
  
  register(user : User)
  {
    this.httpClient.post(`${this.baseUri}/${this.usersUri}/${this.registerUri}`, user).subscribe(() => { }, error => { }, () => {
      this.login(user);
    });
  }

  login(user : User)
  {
    this.httpClient.post(`${this.baseUri}/${this.usersUri}/${this.loginUri}`, user, { responseType: "text"})
    .subscribe(data => {
      localStorage.setItem("accessToken", data);
      this.router.navigateByUrl('./lists');
    });
  }

  getUserDetails(token : string)
  {
    let decodedToken : any = jwtDecode(token);
    let user = new User();
    user.id = decodedToken.sub;
    user.email = decodedToken.email;

    return user;
  }

  getLoginStatus() : boolean
  {
    return localStorage.getItem("accessToken") != null;
  }
}
