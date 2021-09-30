import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../interfaces/IUser';
import { LoginServiceService } from '../services/login-service.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
  
  private loginService : LoginServiceService;
  
  constructor(httpClient : HttpClient, private router : Router, private route : ActivatedRoute) {
    this.registerFormGroup = new FormGroup({
      email: new FormControl(),
      password: new FormControl()
    });

    this.loginService = new LoginServiceService(httpClient, router);
   
  }

  ngOnInit(): void {
    let user : User = new User();
    user.accessToken = localStorage.getItem('accessToken');
  }

  public registerFormGroup : FormGroup;

  register()
  {
    let user : User = new User();
    user.email = this.registerFormGroup.get('email').value;
    user.hashedPassword = this.registerFormGroup.get('password').value;

    let errorData = this.loginService.register(user);

    if (!errorData) {
      this.loginService.login(user);
    }
  }

}
