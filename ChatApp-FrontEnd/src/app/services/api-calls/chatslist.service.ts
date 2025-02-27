import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { userSettingsApi } from '../apipath';
import { catchError, finalize, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ChatsListService {
  private htppClient: HttpClient = inject(HttpClient);
  private isLoading: boolean = false;

  getChatsList() {
    this.isLoading = true;
    return this.htppClient.get<string[]>(`${userSettingsApi}GetAllChats`).pipe(
      finalize(() => (this.isLoading = false)),
      catchError((error) => {
        const errorMessage = error.status;
        return throwError(() => new Error(errorMessage));
      })
    );
  }

  get getIsLoading() {
    return this.isLoading;
  }
}
