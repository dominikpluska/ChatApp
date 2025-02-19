import { Component, inject, OnInit, output } from '@angular/core';
import { SidebarComponentComponent } from './components/sidebar-component/sidebar-component.component';
import { NavbarComponentComponent } from './components/navbar-component/navbar-component.component';
import { Router, RouterOutlet } from '@angular/router';
import { UserSettings } from '../services/usersettings.service';

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [SidebarComponentComponent, NavbarComponentComponent, RouterOutlet],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.css',
})
export class MainPageComponent implements OnInit {
  router = inject(Router);
  userSettings = inject(UserSettings);

  ngOnInit(): void {
    this.userSettings.startSignalRConnection();
    this.userSettings.onNotification();
  }

  onSelect(selection: string) {
    this.router.navigate([`/main/${selection}`]);
  }
}
