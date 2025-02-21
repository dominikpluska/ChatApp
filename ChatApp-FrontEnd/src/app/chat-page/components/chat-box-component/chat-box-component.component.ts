import {
  AfterViewInit,
  Component,
  ElementRef,
  input,
  viewChild,
} from '@angular/core';
import { MessageBoxComponentComponent } from '../message-box-component/message-box-component.component';
import { MessageComponentComponent } from '../message-component/message-component.component';
import { MessageReceived } from '../../../models/messagereceived.model';

@Component({
  selector: 'app-chat-box-component',
  standalone: true,
  imports: [MessageComponentComponent],
  templateUrl: './chat-box-component.component.html',
  styleUrl: './chat-box-component.component.css',
})
export class ChatBoxComponentComponent implements AfterViewInit {
  messages = input<MessageReceived[]>();
  currentUserId = input.required<string>();
  private chatBoxRef = viewChild<ElementRef<HTMLDivElement>>('chatbox');

  //Feature is not ready yet!
  ngAfterViewInit(): void {
    console.log(this.chatBoxRef()!.nativeElement.clientHeight);
    console.log(this.chatBoxRef()!.nativeElement.offsetHeight + ' offset');
  }

  onScroll(event: any) {
    //console.log(event.target);
    //console.log(this.chatBoxRef()?.nativeElement.clientHeight);
    //console.log(this.chatBoxRef()?.nativeElement.scrollTop);
    //console.log(this.chatBoxOverFlowSize);
    // if (event.target.scrollTop === chatBoxOverFlowSize) {
    //   console.log('test!');
    // }
  }
}
