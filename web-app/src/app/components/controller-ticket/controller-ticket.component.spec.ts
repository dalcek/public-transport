import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControllerTicketComponent } from './controller-ticket.component';

describe('ControllerTicketComponent', () => {
  let component: ControllerTicketComponent;
  let fixture: ComponentFixture<ControllerTicketComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControllerTicketComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControllerTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
