import { Component, input, output } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MessagePosted } from '../../../models/messageposted.model';

@Component({
  selector: 'app-message-box-component',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './message-box-component.component.html',
  styleUrl: './message-box-component.component.css',
})
export class MessageBoxComponentComponent {
  form = input<FormGroup<MessagePosted>>();
  onButtonPress = output();

  buttonClick() {
    this.onButtonPress.emit();
  }
}
