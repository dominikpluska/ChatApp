import { Component, DestroyRef, inject } from '@angular/core';
import { BlackListService } from '../../../services/api-calls/blacklist.service';
import { UserSettings } from '../../../services/usersettings.service';
import { NothingToDisplayComponent } from '../../../global-components/nothing-to-display/nothing-to-display.component';
import { TinyItemComponentComponent } from '../../../global-components/tiny-item-component/tiny-item-component.component';
import { TinyButtonComponentComponent } from '../../../global-components/tiny-button-component/tiny-button-component.component';
import { ToastrService } from 'ngx-toastr';
import { LoadingComponentComponent } from '../../../global-components/loading-component/loading-component.component';

@Component({
  selector: 'app-black-list-page',
  standalone: true,
  imports: [
    NothingToDisplayComponent,
    TinyItemComponentComponent,
    TinyButtonComponentComponent,
    LoadingComponentComponent,
  ],
  templateUrl: './black-list-page.component.html',
  styleUrl: './black-list-page.component.css',
})
export class BlackListPageComponent {
  private blackListService = inject(BlackListService);
  private destroyRef = inject(DestroyRef);
  private userSettings = inject(UserSettings);
  private toastr = inject(ToastrService);

  ngOnInit(): void {
    if (this.userSettings.getBlackList == undefined) {
      const subscription = this.blackListService.getBlackList().subscribe({
        next: (response) => {
          this.userSettings.setBlackList = response;
          this.userSettings.onBlockUser();
          this.userSettings.onRemoveBlockedUser();
        },
        error: (error) => {
          console.log(error);
        },
      });
      this.destroyRef.onDestroy(() => subscription.unsubscribe());
    }
  }

  onRemoveFromBlackList(blockedId: string) {
    const subscription = this.blackListService
      .removeUserFromBlackList(blockedId)
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

  get getUserSettings() {
    return this.userSettings;
  }

  get isLoading() {
    return this.blackListService.getIsLoading;
  }
}
