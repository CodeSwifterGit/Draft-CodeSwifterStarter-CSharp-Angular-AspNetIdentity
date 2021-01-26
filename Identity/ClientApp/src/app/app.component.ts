import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountsService } from './services/accounts.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Example App';
  isLoggedIn$: Observable<boolean>;

  constructor(private accountsService: AccountsService) {
    this.isLoggedIn$ = accountsService.isLoggedIn();
  }

  logout() {
    this.accountsService.logout();
  }
}
