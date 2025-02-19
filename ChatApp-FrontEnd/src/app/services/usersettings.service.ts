import { ChangeDetectorRef, inject, Injectable } from '@angular/core';
import { UserProfile } from '../models/userprofile.model';
import { AppRequest } from '../models/apprequest.model';
import { UserLight } from '../models/userlight.model';
import { SignalrRService } from './signalr.service';
import { userSettingsApi } from './apipath';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UserSettings {
  private signalRSevice = inject(SignalrRService);
  private toastr = inject(ToastrService);

  private userProfile: UserProfile = {
    UserId: '',
    UserName: '',
  };

  private friendsList?: UserLight[] | null;

  private chatsList?: string[] | null;

  private blackList?: UserLight[] | null;

  private requestsList?: AppRequest[] | null;

  startSignalRConnection() {
    this.signalRSevice
      .connect(`${userSettingsApi}UserSettings`)
      .then(() => this.toastr.success('Connected to the backend server!'));
  }

  //Terminate SingalRConnection

  onNotification() {
    this.signalRSevice.getHubConnection.on(
      'ReceiveNotification',
      (response: string) => {
        this.toastr.info(response);
      }
    );
  }

  onRequestReceived() {
    this.signalRSevice.getHubConnection.on(
      'OnRequestReceived',
      (request: AppRequest) => {
        if (this.requestsList === undefined || this.requestsList === null) {
          this.requestsList = [request];
        } else {
          this.requestsList!.push(request);
        }
      }
    );
  }

  onRequestRemoved() {
    this.signalRSevice.getHubConnection.on(
      'OnRequestRemoved',
      (requestId: string) => {
        this.requestsList?.splice(
          this.requestsList.findIndex((x) => x.requestId === requestId),
          1
        );
      }
    );
  }

  setUserProfile(id: string, userName: string) {
    this.userProfile.UserId = id;
    this.userProfile.UserName = userName;
  }

  get getUserProfile() {
    return this.userProfile;
  }

  get getFriendsList() {
    return this.friendsList;
  }

  set setFriendsList(friends: UserLight[]) {
    this.friendsList = friends;
  }

  get getChatsList() {
    return this.chatsList;
  }

  set setChatsList(chats: string[]) {
    this.chatsList = chats;
  }

  get getBlackList() {
    return this.blackList;
  }

  set setBlackList(blackList: UserLight[]) {
    this.blackList = blackList;
  }

  get getRequestsList() {
    return this.requestsList;
  }

  set setRequestsList(requests: AppRequest[]) {
    //this.requestsList.next(requests);
    this.requestsList = requests;
  }

  // addRequest(request: AppRequest) {
  //   const currentValue = this.requestsList.value;
  //   this.requestsList.next([...currentValue, request]);
  // }
}
