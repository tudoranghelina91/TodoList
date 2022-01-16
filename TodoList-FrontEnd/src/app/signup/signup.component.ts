import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
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
  private user : User = new User();
  userExists : boolean;

  constructor(httpClient : HttpClient, private router : Router, private route : ActivatedRoute) {
    this.registerFormGroup = new FormGroup ({
      email : new FormControl(this.user.email, [Validators.required, Validators.email, Validators.minLength(6), Validators.maxLength(50)]),
      password : new FormControl(this.user.hashedPassword, [Validators.required, Validators.minLength(4), Validators.maxLength(16)])
    });

    this.loginService = new LoginServiceService(httpClient, router);
  }

  get email() { return this.registerFormGroup.get('email') }
  get password() { return this.registerFormGroup.get('password') }

  ngOnInit(): void {
    let user : User = new User();
  }

  public registerFormGroup : FormGroup;

  register()
  {
    this.user.email = this.email.value;
    this.user.hashedPassword = this.password.value;
    this.loginService.register(this.user);
    this.userExists = true;
  }
}
