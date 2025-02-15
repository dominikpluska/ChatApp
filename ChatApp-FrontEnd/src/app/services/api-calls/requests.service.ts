import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { userSettingsApi } from '../apipath';
import { catchError, throwError } from 'rxjs';
import { AppRequest } from '../../models/apprequest.model';

@Injectable({ providedIn: 'root' })
export class RequestsService {
  private htppClient: HttpClient = inject(HttpClient);

  getRequests() {
    return this.htppClient
      .get<AppRequest[]>(`${userSettingsApi}GetAllRequests`)
      .pipe(
        catchError((error) => {
          //console.log(error.status);
          const errorMessage = error.status;
          return throwError(() => new Error(errorMessage));
        })
      );
  }
}
