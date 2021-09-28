import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { User } from '../interfaces/IUser';
import { LoginServiceService } from '../services/login-service.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  private loginService : LoginServiceService;

  constructor(httpClient : HttpClient) { 
    this.loginFormGroup = new FormGroup ({
      email : new FormControl(),
      password : new FormControl()
    });

    this.registerFormGroup = new FormGroup({
      email: new FormControl(),
      password: new FormControl()
    });

    this.loginService = new LoginServiceService(httpClient);
  }

  public loginFormGroup : FormGroup;
  public registerFormGroup : FormGroup;

  ngOnInit(): void {
  }

  login()
  {
    let user : User = new User();
    user.email = this.loginFormGroup.get('email').value;
    user.hashedPassword = this.loginFormGroup.get('password').value;

    this.loginService.login(user);
  }

  register()
  {
    let user : User = new User();
    user.email = this.registerFormGroup.get('email').value;
    user.hashedPassword = this.registerFormGroup.get('password').value;

    this.loginService.register(user);
  }
}