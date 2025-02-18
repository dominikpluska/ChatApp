import { Component, DestroyRef, inject, input, OnInit } from '@angular/core';
import { ChatBoxComponentComponent } from './components/chat-box-component/chat-box-component.component';
import { MessageBoxComponentComponent } from './components/message-box-component/message-box-component.component';
import { ChatService } from '../services/api-calls/chat.service';
import { ToastrService } from 'ngx-toastr';
import { MessageReceived } from '../models/messagereceived.model';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MessagePosted } from '../models/messageposted.model';
import { UserSettings } from '../services/usersettings.service';
import { UserLight } from '../models/userlight.model';

@Component({
  selector: 'app-chat-page',
  standalone: true,
  imports: [
    ChatBoxComponentComponent,
    MessageBoxComponentComponent,
    ReactiveFormsModule,
  ],
  templateUrl: './chat-page.component.html',
  styleUrl: './chat-page.component.css',
})
export class ChatPageComponent implements OnInit {
  chatId = input.required<string>();
  chatterName = input<string>('TestAccount');

  private userSettings = inject(UserSettings);
  private chatService = inject(ChatService);
  private destroyRef = inject(DestroyRef);
  private toastr = inject(ToastrService);

  textMessageForm = new FormGroup<MessagePosted>({
    ChatId: new FormControl<string>('', [
      Validators.required,
      Validators.minLength(1),
    ]),
    TextMessage: new FormControl<string>('', [
      Validators.required,
      Validators.minLength(1),
    ]),
  });

  ngOnInit(): void {
    if (this.chatId === null) {
      this.toastr.error('Chat Id is not provided!');
    } else {
      this.textMessageForm.get('ChatId')?.setValue(this.chatId());
      this.chatService.getChatMessages(this.chatId());
    }
  }

  async postMessage() {
    await this.chatService.postNewMessage(this.chatId(), this.textMessageForm);
  }

  get getMessages() {
    return this.chatService.getMessages;
  }

  get getCurrentUserId() {
    return this.userSettings.getUserProfile.UserId;
  }

  get getChatParticipants() {
    return this.chatService.getChatParticipants;
  }
}
