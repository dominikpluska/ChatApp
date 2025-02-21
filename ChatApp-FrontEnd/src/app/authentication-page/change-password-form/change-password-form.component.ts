import { Component, DestroyRef, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { NewPassword } from '../../models/newpassword.model';
import { AuthenticationService } from '../../services/api-calls/authentication.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-change-password-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './change-password-form.component.html',
  styleUrl: './change-password-form.component.css',
})
export class ChangePasswordFormComponent {
  private router = inject(Router);
  private wasSubmitted: boolean = false;
  private errorMessage: string = '';
  private destroyRef = inject(DestroyRef);
  private toastr = inject(ToastrService);
  private authenticationService = inject(AuthenticationService);

  newPasswordForm = new FormGroup<NewPassword>({
    CurrentPassword: new FormControl<string>('', [Validators.required]),
    NewPassword: new FormControl<string>('', [Validators.required]),
    ConfirmPassword: new FormControl<string>('', [Validators.required]),
  });

  onRoute() {
    this.router.navigate(['/main']);
  }

  onSubmit() {
    this.wasSubmitted = true;
    if (
      !this.newPasswordForm.get('CurrentPassword')?.errors &&
      !this.newPasswordForm.get('NewPassword')?.errors &&
      !this.newPasswordForm.get('ConfirmPassword')?.errors
    ) {
      const subscription = this.authenticationService
        .changePassword(this.newPasswordForm)
        .subscribe({
          next: (response: any) => {
            this.toastr.success(response);
            this.router.navigate(['/', 'main']);
          },
          error: (error) => {
            this.errorMessage = error;
          },
        });
      this.destroyRef.onDestroy(() => subscription.unsubscribe());
    }
  }

  get getWasSubmitted() {
    return this.wasSubmitted;
  }

  get getErrorMessage() {
    return this.errorMessage;
  }
}
