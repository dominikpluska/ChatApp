import { Component, DestroyRef, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Register } from '../../models/register.model';
import { AuthenticationService } from '../../services/api-calls/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './register-form.component.html',
  styleUrl: './register-form.component.css',
})
export class RegisterFormComponent {
  private sumbitted: boolean = false;
  private wasCreated: boolean = false;
  private errorMessage: string = '';
  private router = inject(Router);
  private authenticationService = inject(AuthenticationService);
  private destroyRef = inject(DestroyRef);

  registerForm = new FormGroup<Register>({
    UserName: new FormControl<string>('', [Validators.required]),
    Email: new FormControl<string>('', [Validators.required]),
    Password: new FormControl<string>('', [Validators.required]),
    ConfirmPassword: new FormControl<string>('', [Validators.required]),
  });

  get getSubmitted() {
    return this.sumbitted;
  }

  get getErrorMessage() {
    return this.errorMessage;
  }

  get getWasCreated() {
    return this.wasCreated;
  }

  onSubmit() {
    this.sumbitted = true;
    if (
      !this.registerForm.get('UserName')?.errors ||
      !this.registerForm.get('Email')?.errors ||
      !this.registerForm.get('Password')?.errors ||
      !this.registerForm.get('ConfirmPassword')?.errors
    ) {
      const subscription = this.authenticationService
        .register(this.registerForm)
        .subscribe({
          next: (response) => {
            this.wasCreated = true;
          },
          error: (error) => {
            console.log(error);
            this.errorMessage = error;
          },
        });
      this.destroyRef.onDestroy(() => subscription.unsubscribe());
    }
  }

  onRoute() {
    this.router.navigate(['/', 'login']);
  }
}
