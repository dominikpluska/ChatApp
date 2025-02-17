import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { NothingToDisplayComponent } from '../../../global-components/nothing-to-display/nothing-to-display.component';
import { TinyItemComponentComponent } from '../../../global-components/tiny-item-component/tiny-item-component.component';
import { TinyButtonComponentComponent } from '../../../global-components/tiny-button-component/tiny-button-component.component';
import { UserSettings } from '../../../services/usersettings.service';
import { RequestsService } from '../../../services/api-calls/requests.service';
import { FriendsListService } from '../../../services/api-calls/friendslists.service';

@Component({
  selector: 'app-requests-page',
  standalone: true,
  imports: [
    NothingToDisplayComponent,
    TinyItemComponentComponent,
    TinyButtonComponentComponent,
  ],
  templateUrl: './requests-page.component.html',
  styleUrl: './requests-page.component.css',
})
export class RequestsPageComponent implements OnInit {
  private requestsService = inject(RequestsService);
  private friendsListService = inject(FriendsListService);
  private destroyRef = inject(DestroyRef);
  private userSettings = inject(UserSettings);

  ngOnInit(): void {
    if (this.userSettings.getRequestsList == undefined) {
      const subscription = this.requestsService.getRequests().subscribe({
        next: (response) => {
          this.userSettings.setRequestsList = response;
        },
        error: (error) => {
          console.log(error);
        },
      });
      this.destroyRef.onDestroy(() => subscription.unsubscribe());
    }
  }

  approveRequest(requestId: string) {
    console.log(requestId);
    const subscription = this.friendsListService
      .approveFriendRequest(requestId)
      .subscribe({
        next: (response) => {
          console.log(response);
        },
        error: (error) => {
          console.log(error);
        },
      });
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  get getUserSettings() {
    return this.userSettings;
  }
}
