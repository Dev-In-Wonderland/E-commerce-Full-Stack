import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ApiManagerService } from 'src/app/services/api-manager.service';

@Component({
  selector: 'app-update-dialog',
  templateUrl: './update-dialog.component.html',
  styleUrls: ['./update-dialog.component.css']
})
export class UpdateDialogComponent {

  middleName: string | null = null;
  phoneNumber: string | null = null;
  userName: string | null = null;
  firstName: string | null = null;
  lastName: string | null = null;
  email: string | null = null;
  isAdmin: boolean = false;

  constructor(public srv: ApiManagerService) {}
}
