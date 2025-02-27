import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { userSettingsApi } from '../apipath';
import { catchError, finalize, throwError } from 'rxjs';
import { UserLight } from '../../models/userlight.model';

@Injectable({ providedIn: 'root' })
export class BlackListService {
  private htppClient: HttpClient = inject(HttpClient);
  private isLoading: boolean = false;

  getBlackList() {
    this.isLoading = true;
    return this.htppClient
      .get<UserLight[]>(`${userSettingsApi}GetBlackList`)
      .pipe(
        finalize(() => (this.isLoading = false)),
        catchError((error) => {
          const errorMessage = error.status;
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  addUserToBlackList(blockedId: string) {
    return this.htppClient
      .post(`${userSettingsApi}AddToBlackList/${blockedId}`, null)
      .pipe(
        catchError((error) => {
          const errorMessage = error.error.detail;
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  removeUserFromBlackList(blockedId: string) {
    return this.htppClient
      .delete(`${userSettingsApi}RemoveFromBlackList/${blockedId}`)
      .pipe(
        catchError((error) => {
          const errorMessage = error;
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  get getIsLoading() {
    return this.isLoading;
  }
}
