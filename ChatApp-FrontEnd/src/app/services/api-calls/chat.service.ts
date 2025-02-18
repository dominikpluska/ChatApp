import { HttpClient } from '@angular/common/http';
import { inject, Injectable, NgZone } from '@angular/core';
import { messageApi } from '../apipath';
import { catchError, throwError } from 'rxjs';
import { MessageReceived } from '../../models/messagereceived.model';
import { FormGroup } from '@angular/forms';
import { MessagePosted } from '../../models/messageposted.model';
import { SignalrRService } from '../signalr.service';
import { UserLight } from '../../models/userlight.model';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private htppClient: HttpClient = inject(HttpClient);
  private signalRSevice = inject(SignalrRService);
  private messages!: MessageReceived[];
  private chatParticipants!: UserLight[];

  getChat(chatterId: string) {
    return this.htppClient
      .get<string>(`${messageApi}OpenChat/${chatterId}`)
      .pipe(
        catchError((error) => {
          const errorMessage = error.error.detail;
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  async getChatMessages(chatId: string) {
    this.signalRSevice.connect(`${messageApi}Chat/${chatId}`).then(() => {
      this.signalRSevice.getHubConnection.invoke('GetMessages', chatId);
    });

    this.signalRSevice.getHubConnection.on('GetMessages', (response: any) => {
      this.chatParticipants = response.users;
      this.messages = response.messages;
    });
  }

  async postNewMessage(
    chatId: string,
    messsagePosted: FormGroup<MessagePosted>
  ) {
    this.signalRSevice.connect(`${messageApi}Chat/${chatId}`).then(() => {
      this.signalRSevice.getHubConnection.invoke(
        'PostMessage',
        messsagePosted.value
      );
    });

    this.signalRSevice.getHubConnection.on(
      'PostMessage',
      (response: MessageReceived) => {
        console.log(response);
        this.messages.push(response);
        messsagePosted.get('TextMessage')?.setValue('');
      }
    );
  }

  get getChatParticipants() {
    return this.chatParticipants;
  }

  get getMessages() {
    return this.messages;
  }
}
