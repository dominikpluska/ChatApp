import { Component } from '@angular/core';
import { ChatBoxComponentComponent } from '../../components/chat-box-component/chat-box-component.component';
import { MessageBoxComponentComponent } from '../../components/message-box-component/message-box-component.component';
import { MessageComponentComponent } from '../../components/message-component/message-component.component';

@Component({
  selector: 'app-chat-page',
  standalone: true,
  imports: [MessageBoxComponentComponent, ChatBoxComponentComponent],
  templateUrl: './chat-page.component.html',
  styleUrl: './chat-page.component.css',
})
export class ChatPageComponent {}
