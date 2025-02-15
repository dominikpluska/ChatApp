import { Injectable } from '@angular/core';
import { UserProfile } from '../models/userprofile.model';
import { Friend } from '../models/friend';
import { AppRequest } from '../models/apprequest.model';

@Injectable({ providedIn: 'root' })
export class UserSettings {
  private userProfile: UserProfile = {
    UserId: '',
    UserName: '',
  };

  private friendsList?: Friend[] | null;

  private chatsList?: string[] | null;

  private blackList?: string[] | null;

  private requestsList?: AppRequest[] | null;

  constructor() {
    //constructor
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

  set setFriendsList(friends: Friend[]) {
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

  set setBlackList(blackList: string[]) {
    this.blackList = blackList;
  }

  get getRequestsList() {
    return this.requestsList;
  }

  set setRequestsList(requests: AppRequest[]) {
    this.requestsList = requests;
  }
}
