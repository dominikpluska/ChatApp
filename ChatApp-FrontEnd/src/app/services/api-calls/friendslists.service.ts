import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { userSettingsApi } from '../apipath';
import { catchError, throwError } from 'rxjs';
import { UserLight } from '../../models/userlight.model';

@Injectable({ providedIn: 'root' })
export class FriendsListService {
  private htppClient: HttpClient = inject(HttpClient);

  getFriendsList() {
    return this.htppClient
      .get<UserLight[]>(`${userSettingsApi}GetAllFriends`)
      .pipe(
        catchError((error) => {
          //console.log(error.status);
          const errorMessage = error.status;
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  sendFriendRequest(userId: string) {
    return this.htppClient
      .post(`${userSettingsApi}SentFriendRequest/${userId}`, null)
      .pipe(
        catchError((error) => {
          //console.log(error.status);
          const errorMessage = error.status;
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  approveFriendRequest(requestId: string) {
    return this.htppClient
      .put(`${userSettingsApi}AcceptFriendRequest/${requestId}`, null)
      .pipe(
        catchError((error) => {
          //console.log(error.status);
          const errorMessage = error.status;
          return throwError(() => new Error(errorMessage));
        })
      );
  }
}
