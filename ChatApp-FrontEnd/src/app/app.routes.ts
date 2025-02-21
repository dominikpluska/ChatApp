import { Routes } from '@angular/router';
import { LoginFormComponent } from './authentication-page/login-form/login-form.component';
import { RegisterFormComponent } from './authentication-page/register-form/register-form.component';
import { MainPageComponent } from './main-page/main-page.component';
import { AuthGuard } from './services/authguard.service';
import { FriendsPageComponent } from './main-page/pages/friends-page/friends-page.component';
import { ChatsPageComponent } from './main-page/pages/chats-page/chats-page.component';
import { BlackListPageComponent } from './main-page/pages/black-list-page/black-list-page.component';
import { RequestsPageComponent } from './main-page/pages/requests-page/requests-page.component';
import { SearchPageComponent } from './main-page/pages/search-page/search-page.component';
import { ChatPageComponent } from './chat-page/chat-page.component';
import { MainViewComponent } from './main-page/pages/main-view/main-view.component';
import { ChangePasswordFormComponent } from './authentication-page/change-password-form/change-password-form.component';
import { ChangeSettingsFormComponent } from './authentication-page/change-settings-form/change-settings-form.component';

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
        path: '',
        component: MainViewComponent,
      },
      {
        path: 'chats',
        component: ChatsPageComponent,
      },
      {
        path: 'friends',
        component: FriendsPageComponent,
      },
      {
        path: 'black-list',
        component: BlackListPageComponent,
      },
      {
        path: 'requests',
        component: RequestsPageComponent,
      },
      {
        path: 'search',
        component: SearchPageComponent,
      },
      {
        path: 'chat',
        component: ChatPageComponent,
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
  {
    path: 'change-password',
    component: ChangePasswordFormComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'change-settings',
    component: ChangeSettingsFormComponent,
    canActivate: [AuthGuard],
  },
];
