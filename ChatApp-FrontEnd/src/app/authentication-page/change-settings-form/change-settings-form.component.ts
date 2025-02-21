import { Component, DestroyRef, inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService } from '../../services/api-calls/authentication.service';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { NewSettings } from '../../models/newsettings.model';

@Component({
  selector: 'app-change-settings-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './change-settings-form.component.html',
  styleUrl: './change-settings-form.component.css',
})
export class ChangeSettingsFormComponent {
  private router = inject(Router);
  private wasSubmitted: boolean = false;
  private errorMessage: string = '';
  private destroyRef = inject(DestroyRef);
  private toastr = inject(ToastrService);
  private authenticationService = inject(AuthenticationService);

  newSettingsForm = new FormGroup<NewSettings>({
    UserName: new FormControl<string>('', [Validators.required]),
    Email: new FormControl<string>('', [Validators.required]),
  });

  onSubmit() {
    this.wasSubmitted = true;
    if (
      !this.newSettingsForm.get('CurrentPassword')?.errors &&
      !this.newSettingsForm.get('NewPassword')?.errors
    ) {
      const subscription = this.authenticationService
        .changeSettings(this.newSettingsForm)
        .subscribe({
          next: (response: any) => {
            this.toastr.success(response);
            this.router.navigate(['/', 'main']);
          },
          error: (error) => {
            this.errorMessage = error.toString();
          },
        });
      this.destroyRef.onDestroy(() => subscription.unsubscribe());
    }
  }

  onRoute() {
    this.router.navigate(['/main']);
  }

  get getWasSubmitted() {
    return this.wasSubmitted;
  }

  get getErrorMessage() {
    return this.errorMessage;
  }
}
