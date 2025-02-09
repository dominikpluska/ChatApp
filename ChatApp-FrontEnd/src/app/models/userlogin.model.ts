import { FormControl } from '@angular/forms';

export interface UserLogin {
  UserName: FormControl<string | null>;
  Password: FormControl<string | null>;
}
