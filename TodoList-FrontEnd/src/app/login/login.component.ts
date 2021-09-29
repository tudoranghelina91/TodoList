import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../interfaces/IUser';
import { LoginServiceService } from '../services/login-service.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  private loginService : LoginServiceService;

  constructor(httpClient : HttpClient, private router : Router, private route : ActivatedRoute) { 
    this.loginFormGroup = new FormGroup ({
      email : new FormControl(),
      password : new FormControl()
    });

    this.loginService = new LoginServiceService(httpClient, router);
  }

  public loginFormGroup : FormGroup;

  ngOnInit(): void {
    let user : User = new User();

    user.accessToken = localStorage.getItem('accessToken');
    this.loginService.login(user);
  }

  login()
  {
    let user : User = new User();
    user.email = this.loginFormGroup.get('email').value;
    user.hashedPassword = this.loginFormGroup.get('password').value;

    this.loginService.login(user);
  }
}
