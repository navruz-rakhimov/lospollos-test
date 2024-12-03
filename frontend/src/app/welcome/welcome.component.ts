import { Component, inject, signal } from '@angular/core';
import { UserService } from '../core/services/user.service';
import { catchError, of } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-welcome',
  imports: [],
  templateUrl: './welcome.component.html',
  styleUrl: './welcome.component.css'
})
export class WelcomeComponent {
  userService = inject(UserService);
  router = inject(Router);
  content = signal<string>('');

  ngOnInit(): void {
    this.userService
    .getHomeInfo()
    .pipe(
      catchError(error => {
        if (error.status === 401 || error.status === 403) {
          this.router.navigateByUrl('/login');
        } else {
          console.log(error);
        }
        return of(null);
      })
    )
    .subscribe(response => {
      if (response) {
        this.content.set(response.welcomeMessage);
      }
    });
  }
}
