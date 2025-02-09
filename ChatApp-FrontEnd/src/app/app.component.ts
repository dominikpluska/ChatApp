import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoadingComponentComponent } from './global-components/loading-component/loading-component.component';
import { MainPageComponent } from './main-page/main-page.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LoadingComponentComponent, MainPageComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'ChatApp-FrontEnd';
}
