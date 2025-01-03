import { AccountService } from './../../../@core/services/account/account.service';
import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss'
})
export class ResetPasswordComponent {
  resetPasswordForm!: FormGroup;
  queryParams: { token: string; userId: string } = { token: '', userId: '' };
  constructor(private fb: FormBuilder, private _accountService: AccountService, private router: Router, private _messageService: MessageService, private route: ActivatedRoute,) {
    this.resetPasswordForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(8)]]
    }, {
      validator: this.passwordMatchValidator
    });
  }
  ngOnInit() {

    this.queryParams = {
      token: this.route.snapshot.queryParams['code'],
      userId: this.route.snapshot.queryParams['userId']
    }
  }

  passwordMatchValidator(group: FormGroup): { [key: string]: boolean } | null {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordMismatch: true };
  }

  resetPassword() {
    if (this.resetPasswordForm.valid) {
      this._accountService.resetPassword(this.resetPasswordForm.value, this.queryParams).subscribe(res => {
        this._messageService.add({ severity: 'success', summary: 'Success', detail: 'Password was set successfully' });
        this.router.navigate(['/auth']);
      }, error => {
        this._messageService.add({ severity: 'error', summary: 'Error', detail: 'Password set failed incorrect password or expired link' });
      });
    }
    alert(JSON.stringify(this.resetPasswordForm.errors));
    this.resetPasswordForm.markAllAsTouched();

  }
}
