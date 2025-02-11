import { Component, input } from '@angular/core';

@Component({
  selector: 'app-tiny-item-component',
  standalone: true,
  imports: [],
  templateUrl: './tiny-item-component.component.html',
  styleUrl: './tiny-item-component.component.css',
})
export class TinyItemComponentComponent {
  componentId = input.required<string>();
  name = input.required<string>();
}
