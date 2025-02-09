import { Component } from '@angular/core';
import { TinyItemComponentComponent } from '../../../global-components/tiny-item-component/tiny-item-component.component';
import { TinyButtonComponentComponent } from '../../../global-components/tiny-button-component/tiny-button-component.component';

@Component({
  selector: 'app-friends-page',
  standalone: true,
  imports: [TinyItemComponentComponent, TinyButtonComponentComponent],
  templateUrl: './friends-page.component.html',
  styleUrl: './friends-page.component.css',
})
export class FriendsPageComponent {}
