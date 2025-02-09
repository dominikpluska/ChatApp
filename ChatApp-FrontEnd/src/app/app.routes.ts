import { Routes } from '@angular/router';
import { LoginFormComponent } from './authentication-page/login-form/login-form.component';
import { RegisterFormComponent } from './authentication-page/register-form/register-form.component';
import { MainPageComponent } from './main-page/main-page.component';
import { AuthGuard } from './services/authguard.service';
import { ChatPageComponent } from './main-page/pages/chat-page/chat-page.component';
import { FriendsPageComponent } from './main-page/pages/friends-page/friends-page.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/main',
    pathMatch: 'full',
  },
  {
    path: 'main',
    component: MainPageComponent,
    children: [
      {
        path: 'chats',
        component: ChatPageComponent,
      },
      {
        path: 'friends',
        component: FriendsPageComponent,
      },
    ],
    canActivate: [AuthGuard],
  },
  {
    path: 'login',
    component: LoginFormComponent,
  },
  {
    path: 'register',
    component: RegisterFormComponent,
  },
];
