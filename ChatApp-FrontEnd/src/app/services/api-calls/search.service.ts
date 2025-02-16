import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { authApi } from '../apipath';
import { catchError, Observable, throwError } from 'rxjs';
import { SearchUser } from '../../models/searchuser.model';

@Injectable({ providedIn: 'root' })
export class SearchService {
  private htppClient = inject(HttpClient);
  private searchUserList?: SearchUser[];
  private userPaginationCount!: number;

  getContacts(itemsToSkip: number = 0) {
    return this.htppClient
      .get<SearchUser[]>(`${authApi}GetActiveUserList/s=${itemsToSkip}`)
      .pipe(
        catchError((error) => {
          const errorMessage = error.status;
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  getUserCountPagination() {
    return this.htppClient
      .get<number>(`${authApi}GetActiveUsersListsDividedBy100`)
      .pipe(
        catchError((error) => {
          const errorMessage = error.status;
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  set setSearchUserList(searchUserList: SearchUser[]) {
    this.searchUserList = searchUserList;
  }

  get getSearchUserList() {
    return this.searchUserList;
  }

  get getUserPagination() {
    return this.userPaginationCount;
  }

  set setUserPaginationCount(paginationCount: number) {
    this.userPaginationCount = paginationCount;
  }
}
