import { Component, inject, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../../core/services/account-service';
import { RegisterCreds } from '../../../types/user';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  protected accountService = inject(AccountService);
  protected creds: RegisterCreds = {
    displayName: '',
    emailAddress: '',
    password: '',
  };

  cancelRegister = output<boolean>();

  register() {
    this.accountService.register(this.creds).subscribe({
      next: (response) => {
        console.log('Rejestracja (i logowanie) udane!', response);
        this.cancelRegister.emit(false);
      },
      error: (error) => {
        alert(error.error?.message || 'Wystąpił błąd podczas rejestracji');
      },
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
