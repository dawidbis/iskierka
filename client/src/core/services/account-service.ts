import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { LoginCreds, RegisterCreds, User } from '../../types/user';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private http = inject(HttpClient);
  currentUser = signal<User | null>(null);

//#region Url strings
  baseUrl = 'https://localhost:5001/api/';
  loginUrl = this.baseUrl + 'account/login';
  registerUrl = this.baseUrl + 'account/register'
//#endregion

//#region Helpers
  private setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.set(user);
  }

  private clearCurrentUser() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }

  private authenticate<T>(url: string, creds: T) {
    return this.http.post<User>(url, creds).pipe(
      tap(user => {
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }
//#endregion

  login(creds: LoginCreds) {
    return this.authenticate<LoginCreds>(this.loginUrl, creds);
  }

  register(creds: RegisterCreds) {
    return this.authenticate<RegisterCreds>(this.registerUrl, creds);
  }

  logout() {
    this.clearCurrentUser();
  }
}
