import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { UsersComponent } from './users/users.component';
import { WelcomeComponent } from './welcome/welcome.component';
import { AuthGuard } from './core/guards/auth.guard';

export const ApiUrl = "https://localhost:7228/api";

export const routes: Routes = [
  { path: 'welcome', component: WelcomeComponent, pathMatch: 'full', canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent, pathMatch: 'full' },
  { path: 'users', component: UsersComponent, pathMatch: 'full'},
];
