import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { LoginUserDTO, ServiceResponse } from 'src/app/models/models';
import { AccountService } from 'src/app/services/account/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
  });

  errorMessage: string;

  // TODO: Better validation
  constructor(private formBuilder: FormBuilder, private accountService: AccountService) { }

  ngOnInit(): void {
    // TODO: call logout or not
  }

  login(){
    if (!this.validateForm()) {
      return;
    }
    this.accountService.login(new LoginUserDTO(this.loginForm.controls.email.value, this.loginForm.controls.password.value)).subscribe(
      result => {
        let jwt = result.data;
        let jwtData = jwt.split('.')[1]
        let decodedJwtData = window.atob(jwtData);
        let decodedJwtDataJSON = JSON.parse(decodedJwtData);

        let role = decodedJwtDataJSON.role;
        let email = decodedJwtDataJSON.email;
        let id = decodedJwtDataJSON.nameid;

        localStorage.setItem('jwt', jwt);
        localStorage.setItem('role', role);
        localStorage.setItem('email', email);
        localStorage.setItem('id', id);
        window.location.href = "/profile";
      },
      err => {
        console.log(err.error.message)
        //window.alert(err.error.message);
        this.errorMessage = 'Incorrect email or password.'
      }
    );
  }

  validateForm(): boolean{
    if (this.loginForm.controls.email.errors) {
      console.log(JSON.stringify(this.loginForm.controls.email.errors));
      if (JSON.stringify(this.loginForm.controls.email.errors).includes('required')) {
        this.errorMessage = 'Email is required.';
      }
      else if (JSON.stringify(this.loginForm.controls.email.errors).includes('email')) {
        this.errorMessage = 'Email format is invalid.';
      }
      return false;
    }
    else if (this.loginForm.controls.password.errors) {
      console.log(this.loginForm.controls.password.errors);
      this.errorMessage = 'Password is required and minimum 8 characters long.';
      return false;
    }
    this.errorMessage = null;
    return true;
  }
}
