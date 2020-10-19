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

export class CreateTicketDTO {
  TicketType: string;
  Email: string;

  constructor (ticketType: string, email: string) {
    this.TicketType = ticketType;
    this.Email = email;
  }
}

export class PaymentDTO {
  TransactionId: string;
  PayerId: string;
  PayerEmail: string;
  TicketId: number;

  constructor (transactionId: string, payerId: string, payerEmail: string, ticketId: number) {
    this.TransactionId = transactionId;
    this.PayerId = payerId;
    this.PayerEmail = payerEmail;
    this.TicketId = ticketId;
  }
}

export class ServiceResponse<T> {
  Data: T;
  Success: Boolean;
  Message: string;
}
