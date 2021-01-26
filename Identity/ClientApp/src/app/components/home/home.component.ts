import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountsService } from 'src/app/services/accounts.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  isLoggedIn$: Observable<boolean>;

  constructor(accountsService: AccountsService) {
    this.isLoggedIn$ = accountsService.isLoggedIn();
  }

  ngOnInit(): void {
  }

}
