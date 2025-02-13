import { Component, DestroyRef, inject } from '@angular/core';
import { BlackListService } from '../../../services/api-calls/blacklist.service';
import { UserSettings } from '../../../services/usersettings.service';
import { NothingToDisplayComponent } from '../../../global-components/nothing-to-display/nothing-to-display.component';
import { TinyItemComponentComponent } from '../../../global-components/tiny-item-component/tiny-item-component.component';
import { TinyButtonComponentComponent } from '../../../global-components/tiny-button-component/tiny-button-component.component';

@Component({
  selector: 'app-black-list-page',
  standalone: true,
  imports: [
    NothingToDisplayComponent,
    TinyItemComponentComponent,
    TinyButtonComponentComponent,
  ],
  templateUrl: './black-list-page.component.html',
  styleUrl: './black-list-page.component.css',
})
export class BlackListPageComponent {
  private blackListService = inject(BlackListService);
  private destroyRef = inject(DestroyRef);
  private userSettings = inject(UserSettings);

  ngOnInit(): void {
    if (this.userSettings.getBlackList == undefined) {
      const subscription = this.blackListService.getBlackList().subscribe({
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
