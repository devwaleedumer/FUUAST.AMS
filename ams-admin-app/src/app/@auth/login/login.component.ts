import { Component } from '@angular/core';
import { LayoutService } from '../../@core/services/layout/layout.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginService } from '../../@core/services/login/login.service';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  loginformGroup!:FormGroup
  Message:string;

  constructor(public layoutService: LayoutService,private fb:FormBuilder,private _service:LoginService,private messageService:MessageService,private route:Router) {
    this.Message='';
   }


ngOnInit(){
  this.loginValidator();
}

loginValidator(){
  this.loginformGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
});
}
loginRequest() {
  debugger
  if (this.loginformGroup.valid) {
    this._service
      .authentication(this.loginformGroup.value) // Ensure this passes correct data, not `this.loginRequest`
      .subscribe((response) => {
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'program Added', life: 3000 });
          const jwttoken = response.token;
          localStorage.setItem('token', jwttoken);
          this.Message="Invalid Credentials"
          this.route.navigateByUrl('/applayout');

      }
    
    );
     this.Message="Invalid Credentials";
  }
  this.loginformGroup.markAllAsTouched(); 
}

}



