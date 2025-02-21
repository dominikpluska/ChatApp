import { FormControl } from '@angular/forms';

export interface NewSettings {
  UserName: FormControl<string | null>;
  Email: FormControl<string | null>;
}
