import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { authApi } from '../apipath';
import { catchError, finalize, throwError } from 'rxjs';
import { SearchUser } from '../../models/searchuser.model';
import { error } from 'console';

@Injectable({ providedIn: 'root' })
export class SearchService {
  private htppClient = inject(HttpClient);
  private searchUserList?: SearchUser[];
  private userPaginationCount!: number;
  private isLoading: boolean = false;

  getContacts(itemsToSkip: number = 0) {
    this.isLoading = true;
    return this.htppClient
      .get<{ userList: SearchUser[]; activeUsersCount: number }>(
        `${authApi}GetActiveUserList/s=${itemsToSkip}`
      )
      .pipe(
        finalize(() => (this.isLoading = false)),
        catchError((error) => {
          const errorMessage = error.error.detail;
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  searchForUser(userName: string) {
    return this.htppClient
      .get<SearchUser[]>(`${authApi}Search/${userName.toLowerCase()}`)
      .pipe(
        catchError((error) => {
          const errorMessage = error.error.detail;
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

  get GetIsLoading() {
    return this.isLoading;
  }
}
