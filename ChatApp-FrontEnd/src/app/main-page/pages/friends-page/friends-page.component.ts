import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { TinyItemComponentComponent } from '../../../global-components/tiny-item-component/tiny-item-component.component';
import { TinyButtonComponentComponent } from '../../../global-components/tiny-button-component/tiny-button-component.component';
import { FriendsListService } from '../../../services/api-calls/friendslists.service';
import { UserSettings } from '../../../services/usersettings.service';
import { NothingToDisplayComponent } from '../../../global-components/nothing-to-display/nothing-to-display.component';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ChatService } from '../../../services/api-calls/chat.service';
import { LoadingComponentComponent } from '../../../global-components/loading-component/loading-component.component';

@Component({
  selector: 'app-friends-page',
  standalone: true,
  imports: [
    TinyItemComponentComponent,
    TinyButtonComponentComponent,
    NothingToDisplayComponent,
    LoadingComponentComponent,
  ],
  templateUrl: './friends-page.component.html',
  styleUrl: './friends-page.component.css',
})
export class FriendsPageComponent implements OnInit {
  private friendsListService = inject(FriendsListService);
  private chatService = inject(ChatService);
  private destroyRef = inject(DestroyRef);
  private userSettings = inject(UserSettings);
  private router = inject(Router);
  private toastr = inject(ToastrService);

  ngOnInit(): void {
    if (this.userSettings.getFriendsList == undefined) {
      const subscription = this.friendsListService.getFriendsList().subscribe({
        next: (response) => {
          this.userSettings.setFriendsList = response;
          this.userSettings.onRemoveFriend();
          this.userSettings.onAddFriend();
        },
        error: (error) => {
          this.toastr.error(error);
          console.log(error);
        },
      });
      this.destroyRef.onDestroy(() => subscription.unsubscribe());
    }
  }

  get getUserSettings() {
    return this.userSettings;
  }

  get isLoading() {
    return this.friendsListService.getIsLoading;
  }

  onChatRoute(userId: string) {
    const subscription = this.chatService.getChat(userId).subscribe({
      next: (response) => {
        this.router.navigate(['main/chat', { chatId: response.toString() }]);
      },
      error: (error) => {
        this.toastr.error(error.toString());
      },
    });
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  onFriendRemove(friendId: string) {
    const subscription = this.friendsListService
      .removeFriend(friendId)
      .subscribe({
        next: (response) => {
          this.toastr.success(response.toString());
        },
        error: (error) => {
          this.toastr.error(error.toString());
        },
      });
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
