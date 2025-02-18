import { Component, input } from '@angular/core';
import { MessageBoxComponentComponent } from '../message-box-component/message-box-component.component';
import { MessageComponentComponent } from '../message-component/message-component.component';
import { MessageReceived } from '../../../models/messagereceived.model';

@Component({
  selector: 'app-chat-box-component',
  standalone: true,
  imports: [MessageComponentComponent],
  templateUrl: './chat-box-component.component.html',
  styleUrl: './chat-box-component.component.css',
})
export class ChatBoxComponentComponent {
  messages = input<MessageReceived[]>();
  currentUserId = input.required<string>();
}
