import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
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
  private user : User = new User();

  showLoginError: boolean;

  constructor(httpClient : HttpClient, private router : Router, private route : ActivatedRoute) { 
    this.loginFormGroup = new FormGroup ({
      email : new FormControl(this.user.email, [Validators.required, Validators.email, Validators.minLength(6), Validators.maxLength(50)]),
      password : new FormControl(this.user.hashedPassword, [Validators.required, Validators.minLength(4), Validators.maxLength(16)])
    });

    this.loginService = new LoginServiceService(httpClient, router);
  }

  public loginFormGroup : FormGroup;

  ngOnInit(): void {
    this.user.accessToken = localStorage.getItem('accessToken');
    this.loginService.login(this.user);
  }

  get email() { return this.loginFormGroup.get('email') }
  get password() { return this.loginFormGroup.get('password') }

  login()
  {
    this.user.email = this.email.value;
    this.user.hashedPassword = this.password.value;
    this.loginService.login(this.user);
    this.showLoginError = true;
  }
}
