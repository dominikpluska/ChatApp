import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { userSettingsApi } from '../apipath';
import { catchError, throwError } from 'rxjs';
import { Friend } from '../../models/friend';

@Injectable({ providedIn: 'root' })
export class FriendsListService {
  private htppClient: HttpClient = inject(HttpClient);

  getFriendsList() {
    return this.htppClient
      .get<Friend[]>(`${userSettingsApi}GetAllFriends`)
      .pipe(
        catchError((error) => {
          //console.log(error.status);
          const errorMessage = error.status;
          return throwError(() => new Error(errorMessage));
        })
      );
  }
}
