import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AccountsService } from 'src/app/services/accounts.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  hide = true;

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
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

  ngOnInit(): void {
  }

  login() {
    if (this.form.valid) {
      this.accountsService.login(this.form.value)
      .then(success => {
        if (success) {
          this.snackbar.open(`Successful`, undefined, {duration: 5000});
          this.router.navigateByUrl('/');
        }
      }).catch(e => {        
        const error = Object.keys(e.error).join(';');
        
        this.snackbar.open(`Not successful (${error})`, undefined, {duration: 5000});
      });
    }
  }

}
