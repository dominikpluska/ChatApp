import { Component } from '@angular/core';
import { MessageBoxComponentComponent } from '../message-box-component/message-box-component.component';
import { MessageComponentComponent } from '../message-component/message-component.component';

@Component({
  selector: 'app-chat-box-component',
  standalone: true,
  imports: [MessageComponentComponent],
  templateUrl: './chat-box-component.component.html',
  styleUrl: './chat-box-component.component.css',
})
export class ChatBoxComponentComponent {}
