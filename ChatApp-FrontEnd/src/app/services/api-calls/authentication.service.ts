import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { UserLogin } from '../../models/userlogin.model';
import { catchError, throwError } from 'rxjs';
import { authApi } from '../apipath';
import { FormGroup } from '@angular/forms';
import { Register } from '../../models/register.model';
import { response } from 'express';
import { error } from 'console';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private htppClient: HttpClient = inject(HttpClient);

  login(userLogin: FormGroup<UserLogin>) {
    return this.htppClient.post(`${authApi}Login`, userLogin.value).pipe(
      catchError((error) => {
        const errorMessage = error.error.detail;
        return throwError(() => new Error(errorMessage));
      })
    );
  }

  register(reigster: FormGroup<Register>) {
    return this.htppClient.post(`${authApi}Register`, reigster.value).pipe(
      catchError((error) => {
        const errorMessage = error.error.detail;
        return throwError(() => new Error(errorMessage));
      })
    );
  }

  checkAuth() {
    return this.htppClient.get(`${authApi}AuthCheck`).pipe(
      catchError((error) => {
        //console.log(error.status);
        const errorMessage = error.status;
        return throwError(() => new Error(errorMessage));
      })
    );
  }
}
