import { Component, OnInit } from '@angular/core';
import { User } from '../interfaces/IUser';
import { LoginServiceService } from '../services/login-service.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  user : User;
  constructor(private loginService : LoginServiceService) { }
  ngOnInit(): void {
    this.user = this.loginService.getUserDetails(localStorage.getItem('accessToken'));
  }
}
