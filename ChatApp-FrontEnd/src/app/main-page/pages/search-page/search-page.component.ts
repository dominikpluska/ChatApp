import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { SearchService } from '../../../services/api-calls/search.service';
import { TinyItemComponentComponent } from '../../../global-components/tiny-item-component/tiny-item-component.component';
import { TinyButtonComponentComponent } from '../../../global-components/tiny-button-component/tiny-button-component.component';
import { BlockList } from 'net';

@Component({
  selector: 'app-search-page',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    TinyItemComponentComponent,
    TinyButtonComponentComponent,
  ],
  templateUrl: './search-page.component.html',
  styleUrl: './search-page.component.css',
})
export class SearchPageComponent implements OnInit {
  private submitted: boolean = false;
  private isSearched: boolean = false;
  private searchService = inject(SearchService);
  private destroyRef = inject(DestroyRef);
  private currentPaginationSelection = 1;

  //implement adding and blocking users from this view
  //implement chat route

  searchPatternForm = new FormGroup({
    searchPattern: new FormControl<string>('', [
      Validators.required,
      Validators.maxLength(25),
    ]),
  });

  ngOnInit(): void {
    if (this.searchService.getSearchUserList === undefined) {
      const subscription = this.searchService.getContacts().subscribe({
        next: (response) => {
          this.searchService.setSearchUserList = response.userList;
          this.searchService.setUserPaginationCount = response.activeUsersCount;
        },
        error: (error) => {
          console.log(error);
        },
      });
      this.destroyRef.onDestroy(() => subscription.unsubscribe());
    }
  }

  get getSearchUserList() {
    return this.searchService.getSearchUserList;
  }

  get getCurrentPaginationSelection() {
    return this.currentPaginationSelection;
  }

  get getPaginationCount() {
    let paginationCount = this.searchService.getUserPagination;
    let fa: number[] = [];
    if (paginationCount < 6) {
      for (let i = 1; i <= paginationCount; i++) {
        fa.push(i);
      }
      return fa;
    } else if (paginationCount > 6 && this.currentPaginationSelection < 4) {
      return (fa = [1, 2, 3, 4, 5, 6, paginationCount]);
    } else if (paginationCount > 6 && this.currentPaginationSelection >= 4) {
      return (fa = [
        1,
        this.currentPaginationSelection - 2,
        this.currentPaginationSelection - 1,
        this.currentPaginationSelection,
        this.currentPaginationSelection + 1,
        this.currentPaginationSelection + 2,
        paginationCount,
      ]);
    }
    return fa;
  }

  onSearch() {
    this.submitted = true;

    const subscription = this.searchService
      .searchForUser(this.searchPatternForm.value.searchPattern!)
      .subscribe({
        next: (response) => {
          if (response.length > 0) {
            this.isSearched = true;
            this.searchService.setSearchUserList = response;
          }
          this.submitted = false;
        },
        error: (error) => {
          console.log(error);
        },
      });
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  get getIsSearched() {
    return this.isSearched;
  }

  get getSubmitted() {
    return this.submitted;
  }

  set setPaginationSelection(pagination: number) {
    this.currentPaginationSelection = pagination;
    const subscription = this.searchService
      .getContacts(pagination - 1)
      .subscribe({
        next: (response) => {
          this.searchService.setSearchUserList = response.userList;
        },
        error: (error) => {
          console.log(error);
        },
      });
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  onClearSearch() {
    const subscription = this.searchService.getContacts().subscribe({
      next: (response) => {
        this.searchService.setSearchUserList = response.userList;
        this.searchService.setUserPaginationCount = response.activeUsersCount;
        this.isSearched = false;
        this.submitted = false;
      },
      error: (error) => {
        console.log(error);
      },
    });
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
