import { inject, Injectable, signal } from '@angular/core';
import { User } from '../interfaces/user.interface';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiUrl } from '../../app.routes';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  currentUserSig = signal<User | undefined | null>(undefined);
  http = inject(HttpClient);

  authenticateUser(username: string, password: string): Observable<User> {
    return this.http
      .post<User>(
        ApiUrl + '/auth/login',
        {
          username: username,
          password: password
        }
      )
  }
}