import { Component } from '@angular/core';
import { LayoutService } from '../../@core/services/layout/layout.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginService } from '../../@core/services/login/login.service';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import { AuthService } from '../../@core/utilities/auth-service.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  loginformGroup!: FormGroup
  Message: string;

  constructor(public layoutService: LayoutService, private fb: FormBuilder, private _service: AuthService, private messageService: MessageService, private route: Router) {
    this.Message = '';
  }


  ngOnInit() {
    this.loginValidator();
  }

  loginValidator() {
    this.loginformGroup = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }
  loginRequest() {
    if (this.loginformGroup.valid) {
      debugger
      this._service
        .login(this.loginformGroup.value) // Ensure this passes correct data, not `this.loginRequest`
        .subscribe((response) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Signin successfully', life: 3000 });
          this._service.setSession(response);
          this.route.navigateByUrl('/applayout');
          debugger
        }, (error) => {
          this.messageService.add({ severity: 'error', summary: 'Failed', detail: error.message || error.detail || 'invalid credentials', life: 3000 });
          // this.Message="Invalid Credentials";
          this.loginformGroup.reset()
        }
        );

    }
    this.loginformGroup.markAllAsTouched();
  }
}



