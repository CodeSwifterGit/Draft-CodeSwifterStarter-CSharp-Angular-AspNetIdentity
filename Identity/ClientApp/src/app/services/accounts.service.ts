import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AccountsService {
  private isLoggedIn$ = new ReplaySubject<boolean>(1);

  constructor(private http: HttpClient) { 
    this.http.get('/api/auth/isloggedin').toPromise()
      .then(isLoggedIn => {
        this.isLoggedIn$.next(!!isLoggedIn);
      });
  }

  register(data: any) {
    return this.http.post('/api/accounts', data, {observe: 'response'})
    .pipe(
      map(response => response.ok)
    )
    .toPromise();
  }

  login(data: any) {
    return this.http.post('/api/auth/login', data, {observe: 'response'})
      .pipe(
        map(response => {
          if (response.ok) {
            this.isLoggedIn$.next(true);
          }

          return response.ok;
        })
      ).toPromise();
  }

  logout() {
    return this.http.post('/api/auth/logout', {}, {observe: 'response'})
      .pipe(
        map(response => {
          if (response.ok) {
            this.isLoggedIn$.next(false);
          }

          return response.ok;
        })
      ).toPromise();
  }

  forgotPassword(data: any) {
    return this.http.post('/api/accounts/forgotpassword', data, {observe: 'response'})
      .pipe(
        map(response => response.ok)
      ).toPromise();
  }

  resetPassword(data: any) {
    return this.http.post('/api/accounts/resetpassword', data, {observe: 'response'})
      .pipe(
        map(response => response.ok)
      ).toPromise();
  }

  isLoggedIn() {
    return this.isLoggedIn$.asObservable();
  }
}
