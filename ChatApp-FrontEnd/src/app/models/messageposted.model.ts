import { FormControl } from '@angular/forms';

export interface MessagePosted {
  ChatId: FormControl<string | null>;
  TextMessage: FormControl<string | null>;
}
