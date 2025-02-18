import { DatePipe, NgClass } from '@angular/common';
import { Component, input } from '@angular/core';

@Component({
  selector: 'app-message-component',
  standalone: true,
  imports: [DatePipe, NgClass],
  templateUrl: './message-component.component.html',
  styleUrl: './message-component.component.css',
})
export class MessageComponentComponent {
  currentuserId = input.required<string>();
  messageId = input.required<string>();
  userId = input.required<string>();
  userName = input.required<string>();
  postedDate = input.required<Date>();
  textMessage = input.required<string>();
}
