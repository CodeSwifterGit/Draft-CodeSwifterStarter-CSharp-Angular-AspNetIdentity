import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AccountsService } from 'src/app/services/accounts.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]]
  });

  get email() {
    return this.form.get('email');
  }

  get password() {
    return this.form.get('password');
  }

  constructor(
    private fb: FormBuilder,
    private accountsService: AccountsService,
    private snackbar: MatSnackBar,
    private router: Router) { }

  reset() {
    if (this.form.valid) {
      this.accountsService.forgotPassword(this.form.value)
        .then(success => {
          if (success) {
            this.snackbar.open("Success", undefined, { duration: 5000 });
            this.router.navigateByUrl('/login');
          }
        })
        .catch(e => {
          const error = Object.keys(e.error).join(';');

          this.snackbar.open(`Not successful (${error})`, undefined, { duration: 5000 });
        });
    }
  }

  ngOnInit(): void {
  }

}
