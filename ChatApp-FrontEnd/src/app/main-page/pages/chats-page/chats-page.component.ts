import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { TinyButtonComponentComponent } from '../../../global-components/tiny-button-component/tiny-button-component.component';
import { ChatsListService } from '../../../services/api-calls/chatslist.service';
import { UserSettings } from '../../../services/usersettings.service';
import { NothingToDisplayComponent } from '../../../global-components/nothing-to-display/nothing-to-display.component';
import { TinyItemComponentComponent } from '../../../global-components/tiny-item-component/tiny-item-component.component';

@Component({
  selector: 'app-chats-page',
  standalone: true,
  imports: [
    TinyButtonComponentComponent,
    NothingToDisplayComponent,
    TinyItemComponentComponent,
  ],
  templateUrl: './chats-page.component.html',
  styleUrl: './chats-page.component.css',
})
export class ChatsPageComponent implements OnInit {
  private chatsListService = inject(ChatsListService);
  private destroyRef = inject(DestroyRef);
  private userSettings = inject(UserSettings);

  ngOnInit(): void {
    if (this.userSettings.getFriendsList == undefined) {
      const subscription = this.chatsListService.getChatsList().subscribe({
        next: (response) => {
          console.log(response);
          this.userSettings.setChatsList = response;
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
