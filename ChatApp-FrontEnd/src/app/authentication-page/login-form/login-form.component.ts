import { Component, DestroyRef, inject, NgZone, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UserLogin } from '../../models/userlogin.model';
import { AuthenticationService } from '../../services/api-calls/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login-form.component.html',
  styleUrl: './login-form.component.css',
})
export class LoginFormComponent implements OnInit {
  private submitted: boolean = false;
  private errorMessage: string = '';
  private authenticationService = inject(AuthenticationService);
  private router = inject(Router);
  private destroyRef = inject(DestroyRef);
  private zone = inject(NgZone);

  ngOnInit(): void {
    this.zone.runOutsideAngular(() => {
      const subscription = this.authenticationService.checkAuth().subscribe({
        next: (response) => {
          this.router.navigate(['/', 'main']);
        },
        error: (error) => {
          console.log(error);
        },
      });
      this.destroyRef.onDestroy(() => subscription.unsubscribe());
    });
  }

  loginForm = new FormGroup<UserLogin>({
    UserName: new FormControl<string>('', [
      Validators.required,
      Validators.minLength(1),
    ]),
    Password: new FormControl<string>('', [
      Validators.required,
      Validators.minLength(1),
    ]),
  });

  get getSubmitted() {
    return this.submitted;
  }

  get getErrorMessage() {
    return this.errorMessage;
  }

  onSubmit() {
    if (
      !this.loginForm.get('Password')?.errors &&
      !this.loginForm.get('UserName')?.errors
    ) {
      const subscription = this.authenticationService
        .login(this.loginForm)
        .subscribe({
          next: (response) => {
            this.submitted = false;
            this.router.navigate(['/', 'main']);
          },
          error: (error) => {
            this.errorMessage = error.ToString();
          },
        });
      this.destroyRef.onDestroy(() => subscription.unsubscribe());
    }
  }

  onRoute() {
    this.router.navigate(['/', 'register']);
  }
}
