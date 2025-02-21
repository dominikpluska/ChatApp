import { Component, DestroyRef, inject, input, output } from '@angular/core';
import { UserSettings } from '../../../services/usersettings.service';
import { AuthenticationService } from '../../../services/api-calls/authentication.service';
import { Router } from '@angular/router';
import { EventEmitter } from 'stream';

@Component({
  selector: 'app-navbar-component',
  standalone: true,
  imports: [],
  templateUrl: './navbar-component.component.html',
  styleUrl: './navbar-component.component.css',
})
export class NavbarComponentComponent {
  userSettings = inject(UserSettings);
  private authenticationService = inject(AuthenticationService);
  private router = inject(Router);
  private isOptionsButtonActive: boolean = false;
  private destroyRef = inject(DestroyRef);

  get getIsOptionsButtonActive() {
    return this.isOptionsButtonActive;
  }

  //detect click outside the component
  onBodyClickDetect() {
    if (this.isOptionsButtonActive === true) {
      this.isOptionsButtonActive = false;
    }
  }

  showOptions() {
    this.isOptionsButtonActive = !this.isOptionsButtonActive;
  }

  logOut() {
    const subscription = this.authenticationService.logout().subscribe({
      next: (response) => {
        this.isOptionsButtonActive = false;
        this.router.navigate(['/', 'login']);
        window.location.reload();
      },
      error: (error) => {
        console.log(error);
      },
    });
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  onRoute(route: string) {
    this.router.navigate([route]);
  }
}
