import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-tiny-button-component',
  standalone: true,
  imports: [],
  templateUrl: './tiny-button-component.component.html',
  styleUrl: './tiny-button-component.component.css',
})
export class TinyButtonComponentComponent {
  buttonType = input.required<
    'delete' | 'chat' | 'approve' | 'accepted' | 'friend' | 'block'
  >();

  select = output();
  value = input<any>();

  onClick() {
    this.select.emit(this.value());
  }
}
