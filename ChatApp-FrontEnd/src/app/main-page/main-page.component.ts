import { Component, inject, output } from '@angular/core';
import { SidebarComponentComponent } from './components/sidebar-component/sidebar-component.component';
import { NavbarComponentComponent } from './components/navbar-component/navbar-component.component';
import { Router, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [SidebarComponentComponent, NavbarComponentComponent, RouterOutlet],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.css',
})
export class MainPageComponent {
  router = inject(Router);
  onBodyClick = output<boolean>();

  onSelect(selection: string) {
    this.router.navigate([`/main/${selection}`]);
  }

  //fix this logic
  onBodyClickDetected() {
    return true;
  }
}
