import { Component, input } from '@angular/core';

@Component({
  selector: 'app-loading-component',
  standalone: true,
  imports: [],
  templateUrl: './loading-component.component.html',
  styleUrl: './loading-component.component.css',
})
export class LoadingComponentComponent {
  text = input<string>('Loading');
}
