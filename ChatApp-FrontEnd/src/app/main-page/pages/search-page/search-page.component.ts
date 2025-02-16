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
import { forkJoin } from 'rxjs';

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
  private searchService = inject(SearchService);
  private destroyRef = inject(DestroyRef);
  private currentPaginationSelection = 1;

  searchPatternForm = new FormGroup({
    searchPattern: new FormControl<string>('', [Validators.required]),
  });

  ngOnInit(): void {
    forkJoin([
      this.searchService.getContacts(),
      this.searchService.getUserCountPagination(),
    ]).subscribe((result) => {
      this.searchService.setSearchUserList = result[0];
      this.searchService.setUserPaginationCount = result[1];
    });
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

  //Work on the search method!
  onSearch() {
    this.submitted = true;
    console.log(this.searchPatternForm.value);
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
          this.searchService.setSearchUserList = response;
        },
        error: (error) => {
          console.log(error);
        },
      });
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
