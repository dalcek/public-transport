export class LoginUserDTO {
  Email: string;
  Password: string;

  constructor (email: string, password: string) {
    this.Email = email;
    this.Password = password;
  }
}

export class AddUserDTO {
  Email: string;
  Password: string;
  Name: string;
  LastName: string;
  DateOfBirth: string;
  UserType: string;
  Photo: any;

  constructor (email: string, name: string, lastName: string, dateOfBirth: string, userType: string, password: string) {
    this.Email = email;
    this.Name = name;
    this.LastName = lastName;
    this.DateOfBirth = dateOfBirth;
    this.UserType = userType;
    this.Password = password;
  }
}

export class GetUserDTO {
  Email: string;
  Name: string;
  LastName: string;
  DateOfBirth: string;
  UserType: string;
  Photo: any;
  UserStatus: string;
}

export class ServiceResponse<T> {
  Data: T;
  Success: Boolean;
  Message: string;
}