import { Component } from '@angular/core';
import { LoginManagerService } from 'src/app/services/login-manager.service';
import {AfterViewInit, ViewChild} from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import { ApiManagerService } from 'src/app/services/api-manager.service';
import { User } from 'src/app/models/user';
import { MatDialog } from '@angular/material/dialog';


@Component({
  selector: 'app-user-manager',
  templateUrl: './user-manager.component.html',
  styleUrls: ['./user-manager.component.css']
})
export class UserManagerComponent implements AfterViewInit{

  middleName: string | null = null;
  phoneNumber: string | null = null;
  userName: string | null = null;
  firstName: string | null = null;
  lastName: string | null = null;
  email: string | null = null;
  password: string | null = null;
  isAdmin: boolean = true;
  
  displayedColumns: string[] = ['Position', 'UserID', 'Name', 'Email', 'Phone', 'Username', 'IsAdmin', 'ModifiedDate', 'Delete/Update'];
  dataSource = new MatTableDataSource<User>(this.usrv.users);
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  user: User | undefined;


  constructor(public srv: LoginManagerService,
              public usrv: ApiManagerService,
              public dialog: MatDialog) {} 

  ngAfterViewInit() {
     this.dataSource.paginator = this.paginator;
  }

  
}


