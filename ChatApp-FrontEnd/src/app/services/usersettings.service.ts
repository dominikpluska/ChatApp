import { Injectable } from '@angular/core';
import { UserProfile } from '../models/userprofile.model';

@Injectable({ providedIn: 'root' })
export class UserSettings {
  private userProfile: UserProfile = {
    UserId: '',
    UserName: '',
  };

  setUserProfile(id: string, userName: string) {
    this.userProfile.UserId = id;
    this.userProfile.UserName = userName;
  }

  get getUserProfile() {
    return this.userProfile;
  }
}
