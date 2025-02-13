import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { userSettingsApi } from '../apipath';
import { catchError, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ChatsListService {
  private htppClient: HttpClient = inject(HttpClient);

  getChatsList() {
    return this.htppClient.get<string[]>(`${userSettingsApi}GetAllChats`).pipe(
      catchError((error) => {
        const errorMessage = error.status;
        return throwError(() => new Error(errorMessage));
      })
    );
  }
}
