import { Component, input } from '@angular/core';

@Component({
  selector: 'app-tiny-button-component',
  standalone: true,
  imports: [],
  templateUrl: './tiny-button-component.component.html',
  styleUrl: './tiny-button-component.component.css',
})
export class TinyButtonComponentComponent {
  buttonType = input.required<'delete' | 'chat' | 'approve'>();
}
