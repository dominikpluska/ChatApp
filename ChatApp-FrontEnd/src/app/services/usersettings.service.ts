import { Injectable } from '@angular/core';
import { UserProfile } from '../models/userprofile.model';
import { Friend } from '../models/friend';

@Injectable({ providedIn: 'root' })
export class UserSettings {
  private userProfile: UserProfile = {
    UserId: '',
    UserName: '',
  };

  private friendsList?: Friend[] | null;

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
}
