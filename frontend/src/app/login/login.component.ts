import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../core/services/auth.service';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  imports: [ReactiveFormsModule],
})
export class LoginComponent {
  fb = inject(FormBuilder);
  authService = inject(AuthService);
  router = inject(Router);
  errorMessage = signal<string|null>(null);

  form = this.fb.nonNullable.group({
    username: ['', Validators.required],
    password: ['', Validators.required],
  });

  onSubmit(): void {
    this.authService
      .authenticateUser(this.form.getRawValue().username, this.form.getRawValue().password)
      .pipe(
        catchError(error => {
          if (error.status === 401 || error.status === 403) {
            this.errorMessage.set('We could not log you in. Please check your username/password and try again.');
          } else {
            this.errorMessage.set('An error occurred. Please try again.');
          }
          return of(null);
        })
      )
      .subscribe(response => {
        if (response) {
          localStorage.setItem('token', response.token);
          this.authService.currentUserSig.set({
            id: response.id,
            isActive: response.isActive,
            username: response.username,
            token: response.token 
          });
          this.router.navigateByUrl('/welcome');
        }
      });
  }
}
