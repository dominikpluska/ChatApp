import { FormControl } from '@angular/forms';

export interface Register {
  UserName: FormControl<string | null>;
  Email: FormControl<string | null>;
  Password: FormControl<string | null>;
  ConfirmPassword: FormControl<string | null>;
}
