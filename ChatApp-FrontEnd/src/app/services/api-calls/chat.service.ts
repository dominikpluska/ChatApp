import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { messageApi } from '../apipath';
import { catchError, throwError } from 'rxjs';
import { MessageReceived } from '../../models/messagereceived.model';
import { FormGroup } from '@angular/forms';
import { MessagePosted } from '../../models/messageposted.model';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private htppClient: HttpClient = inject(HttpClient);

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
}
