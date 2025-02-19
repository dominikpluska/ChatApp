import { HttpClient } from '@angular/common/http';
import { inject, Injectable, NgZone } from '@angular/core';
import { messageApi } from '../apipath';
import { catchError, throwError } from 'rxjs';
import { MessageReceived } from '../../models/messagereceived.model';
import { FormGroup } from '@angular/forms';
import { MessagePosted } from '../../models/messageposted.model';
import { SignalrRService } from '../signalr.service';
import { UserLight } from '../../models/userlight.model';
import { ToastrService } from 'ngx-toastr';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private htppClient: HttpClient = inject(HttpClient);
  private signalRSevice = inject(SignalrRService);
  private messages!: MessageReceived[];
  private chatParticipants!: UserLight[];
  private toastr = inject(ToastrService);

  startSignalRConnection(chatId: string) {
    this.signalRSevice
      .connect(`${messageApi}Chat`)
      .then(() =>
        this.signalRSevice.getHubConnection.invoke('JoinChat', chatId)
      );
  }

  terminateSignalRConnection(chatId: string) {
    this.signalRSevice.getHubConnection.invoke('LeaveChat', chatId);
  }

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

  postNewMessage(messsagePosted: FormGroup<MessagePosted>) {
    return this.htppClient
      .post<MessageReceived[]>(`${messageApi}PostMessage`, messsagePosted.value)
      .pipe(
        catchError((error) => {
          const errorMessage = error.error.detail;
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  onMessageReceived() {
    this.signalRSevice.getHubConnection.on(
      'ReceiveMessage',
      (response: MessageReceived) => {
        this.messages.push(response);
      }
    );
  }

  getChatMessages(chatId: string) {
    return this.htppClient
      .get<any>(`${messageApi}GetChatMessages/${chatId}`)
      .pipe(
        catchError((error) => {
          const errorMessage = error.error.detail;
          return throwError(() => new Error(errorMessage));
        })
      );
  }

  get getChatParticipants() {
    return this.chatParticipants;
  }

  get getMessages() {
    return this.messages;
  }

  set setMessages(messages: MessageReceived[]) {
    this.messages = messages;
  }

  set setChatParticipants(chatParticipants: UserLight[]) {
    this.chatParticipants = chatParticipants;
  }
}
