import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { LoginUserDTO } from 'src/app/models/models';
import { AccountService } from 'src/app/services/account.service';

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

  incorrectCredentials: boolean = false;

  constructor(private formBuilder: FormBuilder, private accountService: AccountService) { }

  ngOnInit(): void {
    // TODO: call logout
  }

  login(){
    this.accountService.login(new LoginUserDTO(this.loginForm.controls.email.value, this.loginForm.controls.password.value)).subscribe(
      result => {
        console.log(result);
      }
    );
  }
}
