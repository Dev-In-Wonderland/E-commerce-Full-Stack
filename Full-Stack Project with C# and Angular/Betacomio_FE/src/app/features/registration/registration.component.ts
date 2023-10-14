import { Component } from '@angular/core';
import { LoginManagerService } from 'src/app/services/login-manager.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {

  middleName: string | null = null;
  phoneNumber: string | null = null;
  userName: string | null = null;
  firstName: string | null = null;
  lastName: string | null = null;
  email: string | null = null;
  password: string | null = null;


  constructor(public srv: LoginManagerService) {}
}
