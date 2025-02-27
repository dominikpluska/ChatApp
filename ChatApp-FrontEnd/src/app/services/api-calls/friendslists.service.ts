import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { userSettingsApi } from '../apipath';
import { catchError, finalize, throwError } from 'rxjs';
import { UserLight } from '../../models/userlight.model';
@Injectable({ providedIn: 'root' })
export class FriendsListService {
  private htppClient: HttpClient = inject(HttpClient);
  private isLoading: boolean = false;

  getFriendsList() {
    this.isLoading = true;
    return this.htppClient
      .get<UserLight[]>(`${userSettingsApi}GetAllFriends`)
      .pipe(
        finalize(() => (this.isLoading = false)),
        catchError((error) => {
          const errorMessage = error.error.detail;
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  sendFriendRequest(userId: string) {
    return this.htppClient
      .post(`${userSettingsApi}SentFriendRequest/${userId}`, null)
      .pipe(
        catchError((error) => {
          const errorMessage = error.error.detail;
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  approveFriendRequest(requestId: string) {
    return this.htppClient
      .put(`${userSettingsApi}AcceptFriendRequest/${requestId}`, null)
      .pipe(
        catchError((error) => {
          const errorMessage = error.error.detail;
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  rejectFriendRequest(requestId: string) {
    return this.htppClient
      .delete(`${userSettingsApi}RejectFriendRequest/${requestId}`)
      .pipe(
        catchError((error) => {
          const errorMessage = error.error.detail;
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  removeFriend(friendId: string) {
    return this.htppClient
      .delete(`${userSettingsApi}RemoveFriend/${friendId}`)
      .pipe(
        catchError((error) => {
          const errorMessage = error.error.detail;
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  get getIsLoading() {
    return this.isLoading;
  }
}
