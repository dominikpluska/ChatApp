import { Component, inject } from '@angular/core';
import { UserSettings } from '../../../services/usersettings.service';

@Component({
  selector: 'app-navbar-component',
  standalone: true,
  imports: [],
  templateUrl: './navbar-component.component.html',
  styleUrl: './navbar-component.component.css',
})
export class NavbarComponentComponent {
  userSettings = inject(UserSettings);

  onClick() {
    console.log(this.userSettings.getUserProfile.UserName);
    console.log();
  }
}
