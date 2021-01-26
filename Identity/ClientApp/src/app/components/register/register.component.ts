import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AccountsService } from 'src/app/services/accounts.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  hide = true;

  form = this.fb.group({
    firstName: ['', Validators.required],
    surname: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });
  
  get firstName() {
    return this.form.get('firstName');
  }

  get surname() {
    return this.form.get('surname');
  }

  get email() {
    return this.form.get('email');
  }

  get password() {
    return this.form.get('password');
  }

  constructor(
    private fb: FormBuilder,
    private accountsService: AccountsService,
    private router: Router,
    private snackbar: MatSnackBar) { }

  ngOnInit(): void {
  }

  submit() {
    if (this.form.valid) {
      this.accountsService.register(this.form.value)
      .then(success => {
        if (success) {
          this.snackbar.open("Successful", undefined, {duration: 5000});
          this.router.navigateByUrl('/login');
        }
      }).catch(e => {
        const error = Object.keys(e.error).join(';');
        
        this.snackbar.open(`Not successful (${error})`, undefined, {duration: 5000});
      });
    }
  }

}
