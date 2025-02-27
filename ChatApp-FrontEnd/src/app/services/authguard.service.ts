import { CanActivate, Router } from '@angular/router';
import { catchError, map, Observable, of } from 'rxjs';
import { AuthenticationService } from './api-calls/authentication.service';
import { inject, Injectable } from '@angular/core';
import { UserSettings } from './usersettings.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  private authenticationService = inject(AuthenticationService);
  private userSettingsService = inject(UserSettings);

  private router = inject(Router);
  canActivate(): Observable<boolean> {
    return this.authenticationService.checkAuth().pipe(
      map((response: any) => {
        this.userSettingsService.setUserProfile(response.userId, response.user);
        return true;
      }),
      catchError((error) => {
        this.router.navigate(['/', 'login']);
        return of(false);
      })
    );
  }
}
