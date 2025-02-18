import { inject, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';

@Injectable({ providedIn: 'root' })
export class SignalrRService {
  private hubConnection!: HubConnection;
  private toastr = inject(ToastrService);

  //   constructor() {
  //     this.hubConnection = new HubConnectionBuilder()
  //       .withUrl('https://localhost:7501')
  //       .build();
  //   }

  get getHubConnection() {
    return this.hubConnection;
  }

  async connect(url: string): Promise<void> {
    try {
      this.hubConnection = new HubConnectionBuilder().withUrl(url).build();
      await this.hubConnection.start();
    } catch (error) {
      console.log(error);
    }
  }
}
