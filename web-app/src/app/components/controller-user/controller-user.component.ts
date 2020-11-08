import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { GetUserDTO } from 'src/app/models/models';
import { AccountService } from 'src/app/services/account/account.service';

@Component({
  selector: 'app-controller-user',
  templateUrl: './controller-user.component.html',
  styleUrls: ['./controller-user.component.css']
})
export class ControllerUserComponent implements OnInit {

   profileForm = this.formBuilder.group({
      userSelect: ['choose'],
      name: ['', {disabled: true}],
      lastName: ['', {disabled: true}],
      email: ['', {disabled: true}],
      dateOfBirth: ['', {disabled: true}],
      userType: ['', {disabled: true}],
      userStatus: ['', {disabled: true}],
      photo: ['']
    });//, {validator: this.checkPassword});

   photoPath: string;
   users: GetUserDTO[] = [];
   userEmail: string = '';

   constructor(private formBuilder: FormBuilder, private accountService: AccountService) { }

   ngOnInit(): void {
      this.disableFormEdit();
      this.getUsers();
   }

   disableFormEdit() {
      this.profileForm.get('name').disable();
      this.profileForm.get('lastName').disable();
      this.profileForm.get('email').disable();
      this.profileForm.get('dateOfBirth').disable();
      this.profileForm.get('userType').disable();
      this.profileForm.get('userStatus').disable();
   }

   clearForm() {
      this.profileForm.controls.name.setValue('');
      this.profileForm.controls.lastName.setValue('');
      this.profileForm.controls.email.setValue('');
      this.profileForm.controls.userType.setValue('');
      this.profileForm.controls.userStatus.setValue('');
      this.profileForm.controls.dateOfBirth.setValue('');
   }

   getUsers() {
      this.accountService.getUnvalidatedUsers().subscribe(
         result => {
            console.log(result)
            this.users = result.data;
            this.userEmail = '';
            this.clearForm();
            if (this.users.length != 0) {
               this.profileForm.controls.userSelect.setValue('choose');
            }
            else {
               this.profileForm.controls.userSelect.setValue('no');
            }
         },
         err => {
            console.log(err.error.message);
         }
      );
   }

   validate() {
      this.accountService.validate(this.userEmail, true).subscribe(
         result => {
            this.getUsers();
         },
         err => {
            console.log(err.error.message);
         }
      );
      this.photoPath = '';
   }

   deny() {
      this.accountService.validate(this.userEmail, false).subscribe(
         result => {
            this.getUsers();
         },
         err => {
            console.log(err.error.message);
         }
      );
      this.photoPath = '';
   }

   onSelect(event: any) {
      if (event.target.value != 'choose' && event.target.value != 'no') {
         this.userEmail = event.target.value;
         for (let i = 0; i < this.users.length; i++) {
            if (this.users[i]['email'] == this.userEmail) {
               this.profileForm.controls.name.setValue(this.users[i]['name']);
               this.profileForm.controls.lastName.setValue(this.users[i]['lastName']);
               this.profileForm.controls.email.setValue(this.users[i]['email']);
               this.profileForm.controls.userType.setValue(this.users[i]['userType']);
               this.profileForm.controls.userStatus.setValue(this.users[i]['userStatus']);
               let fullDate = this.users[i]['dateOfBirth'].split(' ')[0].split('/');
               let formattedDate = `${fullDate[2]}-${fullDate[0]}-${fullDate[1]}`;
               this.profileForm.controls.dateOfBirth.setValue(formattedDate);
               this.photoPath = `http://localhost:6001/${this.users[i]['photo']}`;
            }
         }
      }
   }
}
