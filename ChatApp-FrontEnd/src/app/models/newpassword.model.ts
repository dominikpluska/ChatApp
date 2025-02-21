import { FormControl } from '@angular/forms';

export interface NewPassword {
  CurrentPassword: FormControl<string | null>;
  NewPassword: FormControl<string | null>;
  ConfirmPassword: FormControl<string | null>;
}
