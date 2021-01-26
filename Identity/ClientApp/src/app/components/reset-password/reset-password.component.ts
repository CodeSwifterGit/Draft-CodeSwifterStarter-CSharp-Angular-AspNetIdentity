import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountsService } from 'src/app/services/accounts.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  hide = true;

  form = this.fb.group({
    newPassword: ['', [Validators.required, Validators.minLength(6)]],
    token: [''],
    userId: ['']
  });

  get password() {
    return this.form.get('password');
  }
  
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private snackbar: MatSnackBar,
    private accountsService: AccountsService) {}

  ngOnInit(): void {
    this.form.patchValue({
      userId: this.route.snapshot.queryParams.userId,
      token: this.route.snapshot.queryParams.token
    });
  }

  reset() {
    if (this.form.valid) {
      this.accountsService.resetPassword(this.form.value)
      .then(success => {
        if (success) {
          this.snackbar.open('Success', undefined, {duration: 5000});
          this.router.navigateByUrl('/login');
        }
      })
      .catch(e => {        
        const error = Object.keys(e.error).join(';');

        this.snackbar.open(`Not successful (${error})`, undefined, {duration: 5000});
      })
    }
  }
}
