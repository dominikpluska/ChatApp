import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { TinyItemComponentComponent } from '../../../global-components/tiny-item-component/tiny-item-component.component';
import { TinyButtonComponentComponent } from '../../../global-components/tiny-button-component/tiny-button-component.component';
import { FriendsListService } from '../../../services/api-calls/friendslists.service';
import { UserSettings } from '../../../services/usersettings.service';
import { NothingToDisplayComponent } from '../../../global-components/nothing-to-display/nothing-to-display.component';

@Component({
  selector: 'app-friends-page',
  standalone: true,
  imports: [
    TinyItemComponentComponent,
    TinyButtonComponentComponent,
    NothingToDisplayComponent,
  ],
  templateUrl: './friends-page.component.html',
  styleUrl: './friends-page.component.css',
})
export class FriendsPageComponent implements OnInit {
  private friendsListService = inject(FriendsListService);
  private destroyRef = inject(DestroyRef);
  private userSettings = inject(UserSettings);

  //Implement a friends list to the userSettings using friend model

  ngOnInit(): void {
    if (this.userSettings.getFriendsList == undefined) {
      const subscription = this.friendsListService.getFriendsList().subscribe({
        next: (response) => {
          console.log(response);
          this.userSettings.setFriendsList = response;
        },
        error: (error) => {
          console.log(error);
        },
      });
      this.destroyRef.onDestroy(() => subscription.unsubscribe());
    }
  }

  get getUserSettings() {
    return this.userSettings;
  }
}
