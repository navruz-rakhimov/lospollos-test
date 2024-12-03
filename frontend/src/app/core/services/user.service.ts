import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { ApiUrl } from "../../app.routes";
import { Observable } from "rxjs";
import { Home } from "../interfaces/home.interface";
import { User } from "../interfaces/user.interface";

@Injectable({
    providedIn: 'root',
})
export class UserService {
  private usersUrl = ApiUrl + '/users';
  private setActiveUrl = ApiUrl + '/users/set-activity-status';

  http = inject(HttpClient);

  getHomeInfo(): Observable<Home> {
    return this.http.get<Home>(ApiUrl + '/home/welcome');
  }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.usersUrl);
  } 

  setUserActivity(userId: number, isActive: boolean): Observable<any> {
    return this.http.post(`${this.setActiveUrl}/${userId}`, { IsActive: isActive });
  }
}
