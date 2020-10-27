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

export class PricelistDTO {
  From: string;
  To: string;
  HourPrice: number;
  DayPrice: number;
  MonthPrice: number;
  YearPrice: number;

  constructor (from: string, to: string, hourPrice: number, dayPrice: number, monthPrice: number, yearPrice: number) {
    this.From = from;
    this.To = to;
    this.HourPrice = hourPrice;
    this.MonthPrice = monthPrice;
    this.YearPrice = yearPrice;
  }
}

export class AddDepartureDTO {
   Id: number;
   Time: string;
   TimetableId: number;

   constructor (id: number, time: string, timetableId: number) {
      this.Id = id;
      this.Time = time;
      this.TimetableId = timetableId;
   }
}

export class DepartureDTO {
   public Id: number;
   public Time: string;
}

export class LineNameDTO {
   Id: number;
   Name: string;
}

export class StationDTO {
   Id: number;
   Name: string;

   constructor (id: number, name: string) {
      this.Id = id;
      this.Name = name;
   }
}

export class AddStationDTO {
   Name: string;
   Address: string;
   XCoordinate: number;
   YCoordinate: number;

   constructor (name: string, address: string, xCoordinate: number, yCoordinate: number) {
      this.Name = name;
      this.Address = address;
      this.XCoordinate = xCoordinate;
      this.YCoordinate = yCoordinate;
   }
}

export class Station {
   Id: number;
   Name: string;
   Address: string;
   XCoordinate: number;
   YCoordinate: number;
   LineStations: any;

   constructor (id: number, name: string, address: string, xCoordinate: number, yCoordinate: number) {
      this.Id = id;
      this.Name = name;
      this.Address = address;
      this.XCoordinate = xCoordinate;
      this.YCoordinate = yCoordinate;
   }
}

export class LineDTO {
   Id: number;
   Name: string;
   Type: string;
   StationIds: any[];

   constructor (id: number, name: string, type: string, stationIds: any[]) {
      this.Id = id;
      this.Name = name;
      this.Type = type;
      this.StationIds = stationIds;
   }
}

export class ServiceResponse<T> {
  Data: T;
  Success: Boolean;
  Message: string;
}
