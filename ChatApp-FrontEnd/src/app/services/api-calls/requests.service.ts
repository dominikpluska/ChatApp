import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { userSettingsApi } from '../apipath';
import { catchError, finalize, throwError } from 'rxjs';
import { AppRequest } from '../../models/apprequest.model';

@Injectable({ providedIn: 'root' })
export class RequestsService {
  private htppClient: HttpClient = inject(HttpClient);
  private isLoading: boolean = false;

  getRequests() {
    this.isLoading = true;
    return this.htppClient
      .get<AppRequest[]>(`${userSettingsApi}GetAllRequests`)
      .pipe(
        finalize(() => (this.isLoading = false)),
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
