import { WrappedNodeExpr } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AddUserDTO, LoginUserDTO } from 'src/app/models/models';
import { AccountService } from 'src/app/services/account/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerForm = this.formBuilder.group({
    name: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    address: ['', Validators.required],
    dateOfBirth: ['', Validators.required],
    userType: ['RegularUser', Validators.required],
    password: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', [Validators.required, Validators.minLength(8)]],
    photo: [''],
  }, {validator: this.checkPassword});

  user: AddUserDTO;
  photoFile: any;
  formData: any;
  photoMessage: string = 'Choose photo';
  errorMessage: string;

  constructor(private formBuilder: FormBuilder, private accountService: AccountService) { }

  ngOnInit(): void {
  }

  populateUser() {
    this.user = new AddUserDTO(this.registerForm.controls.email.value, this.registerForm.controls.name.value,
      this.registerForm.controls.lastName.value, this.registerForm.controls.dateOfBirth.value, this.registerForm.controls.userType.value, this.registerForm.controls.password.value);
  }

  register() {
    if (!this.validateForm()) {
      return;
    }
    this.errorMessage = '';
    this.populateUser();
    this.accountService.register(this.user).subscribe(
      result => {
        this.login();
        window.alert('Successfully registered!');

        if (this.photoFile != null) {
          let formData = new FormData();
          formData.append('image', this.photoFile, this.photoFile.name);
          formData.append('id', result.data);

          this.accountService.uploadImage(formData).subscribe(
            result => {
            },
            err => {
              console.log(err.error.message);
              window.alert('Photo upload failed. Try again at profile tab.');
            }
          );
        }
        window.location.href = "/profile";
      },
      err => {
        console.log(err.error.message);
        window.alert(err.error.message);
      }
    );
  }

  onImageChange(event){
    this.photoFile = <File>event.target.files[0];
    this.photoMessage = this.photoFile.name;
  }

  onSelect(event : any) {
    //this.user.UserType = event.target.value;
  }

  checkPassword(group: FormGroup)
  {
    let password = group.controls.password.value;
    let confirmPassword = group.controls.confirmPassword.value;
    return password == confirmPassword ? false : true;
  }
  // TODO: share this method from login component instead of copying the code
  login(){
    this.accountService.login(new LoginUserDTO(this.registerForm.controls.email.value, this.registerForm.controls.password.value)).subscribe(
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
      },
      err => {
        console.log(err.error.message)
        window.alert('Login failed. Try again.');
        window.location.href = '/login';
      }
    );
  }

  validateForm(): boolean{
    if (this.registerForm.controls.name.errors) {
      this.errorMessage = 'Name is required.';
      return false;
    }
    else if (this.registerForm.controls.lastName.errors) {
      this.errorMessage = 'Last name is required.';
      return false;
    }
    else if (this.registerForm.controls.email.errors) {
      console.log(JSON.stringify(this.registerForm.controls.email.errors));
      if (JSON.stringify(this.registerForm.controls.email.errors).includes('required')) {
        this.errorMessage = 'Email is required.';
      }
      else if (JSON.stringify(this.registerForm.controls.email.errors).includes('email')) {
        this.errorMessage = 'Email format is invalid.';
      }
      return false;
    }
    else if (this.registerForm.controls.password.errors) {
      console.log(this.registerForm.controls.password.errors);
      this.errorMessage = 'Password is required and minimum 8 characters long.';
      return false;
    }
    else if (this.checkPassword(this.registerForm)) {
      this.errorMessage = 'Password and confirm password don\'t match.';
      return false;
    }
    else if (this.registerForm.controls.dateOfBirth.errors) {
      console.log(this.registerForm.controls.dateOfBirth.errors);
      this.errorMessage = 'Date of birth is required.';
      return false;
    }
    return true;
  }
}
