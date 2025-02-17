import { Component, input, OnInit } from '@angular/core';
import { ChatBoxComponentComponent } from '../main-page/components/chat-box-component/chat-box-component.component';
import { MessageBoxComponentComponent } from '../main-page/components/message-box-component/message-box-component.component';

@Component({
  selector: 'app-chat-page',
  standalone: true,
  imports: [ChatBoxComponentComponent, MessageBoxComponentComponent],
  templateUrl: './chat-page.component.html',
  styleUrl: './chat-page.component.css',
})
export class ChatPageComponent implements OnInit {
  chatId = input.required<string>();

  ngOnInit(): void {
    console.log(this.chatId);
  }
}
