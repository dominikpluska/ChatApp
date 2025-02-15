import { Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-search-page',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './search-page.component.html',
  styleUrl: './search-page.component.css',
})
export class SearchPageComponent {
  private submitted: boolean = false;
  searchPatternForm = new FormGroup({
    searchPattern: new FormControl<string>('', [Validators.required]),
  });

  //Implement pagination

  onSearch() {
    this.submitted = true;
    console.log(this.searchPatternForm.value);
  }

  get getSubmitted() {
    return this.submitted;
  }
}
