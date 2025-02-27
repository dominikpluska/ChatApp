import {
  Component,
  computed,
  DestroyRef,
  ElementRef,
  inject,
  input,
  OnDestroy,
  OnInit,
  viewChild,
} from '@angular/core';
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
export class ChatPageComponent implements OnInit, OnDestroy {
  chatId = input.required<string>();

  private userSettings = inject(UserSettings);
  private chatService = inject(ChatService);
  private destroyRef = inject(DestroyRef);
  private toastr = inject(ToastrService);

  //prevent from sending empty messages
  textMessageForm = new FormGroup<MessagePosted>({
    ChatId: new FormControl<string>('', [
      Validators.required,
      Validators.minLength(1),
    ]),
    TextMessage: new FormControl<string>('', [
      Validators.required,
      Validators.minLength(2),
    ]),
  });

  ngOnInit(): void {
    if (this.chatId === null) {
      this.toastr.error('Chat Id is not provided!');
    } else {
      const subscription = this.chatService
        .getChatMessages(this.chatId()!)
        .subscribe({
          next: (response) => {
            this.textMessageForm.get('ChatId')?.setValue(this.chatId());
            this.chatService.setMessages = response.messages;
            this.chatService.setChatParticipants = response.users;
            this.chatService.startSignalRConnection(this.chatId());
            this.chatService.onMessageReceived();
          },
          error: (error) => {
            this.toastr.error(error.toString());
          },
        });
      this.destroyRef.onDestroy(() => subscription.unsubscribe());
    }
  }

  ngOnDestroy() {
    this.chatService.terminateSignalRConnection(this.chatId());
  }

  postMessage() {
    if (
      !this.textMessageForm.get('ChatId')?.errors &&
      !this.textMessageForm.get('TextMessage')?.errors
    ) {
      const subscription = this.chatService
        .postNewMessage(this.textMessageForm)
        .subscribe({
          next: (response) => {
            this.textMessageForm.get('TextMessage')?.setValue('');
          },
          error: (error) => {
            this.toastr.error(error.toString());
          },
        });
      this.destroyRef.onDestroy(() => subscription.unsubscribe());
    } else {
      this.toastr.error('Message must not be empty!');
    }
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

  get getIsLoading() {
    return this.chatService.getIsLoading;
  }
}
