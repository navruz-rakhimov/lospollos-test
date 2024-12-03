import { Component, OnInit } from '@angular/core';
import { UserService } from '../core/services/user.service';
import { User } from '../core/interfaces/user.interface';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-list',
  templateUrl: './users.component.html',
  imports: [FormsModule],
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: User[] = [];
  selectedUser: User | null = null;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.userService.getUsers().subscribe(data => {
      this.users = data;
    });
  }

  onSelect(user: any): void {
    this.selectedUser = { ...user };
  }

  updateUserActivity(): void {
    if (this.selectedUser) {
      this.userService.setUserActivity(this.selectedUser.id, this.selectedUser.isActive)
        .subscribe(() => {
          const userIndex = this.users.findIndex(u => u.id === this.selectedUser!.id);
          this.users[userIndex].isActive = this.selectedUser!.isActive;
          this.selectedUser = null;
        });
    }
  }
}
