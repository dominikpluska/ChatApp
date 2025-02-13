import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { userSettingsApi } from '../apipath';
import { catchError, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class BlackListService {
  private htppClient: HttpClient = inject(HttpClient);

  getBlackList() {
    return this.htppClient.get<string[]>(`${userSettingsApi}GetBlackList`).pipe(
      catchError((error) => {
        const errorMessage = error.status;
        return throwError(() => new Error(errorMessage));
      })
    );
  }
}
