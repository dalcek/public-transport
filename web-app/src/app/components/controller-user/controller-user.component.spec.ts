import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControllerUserComponent } from './controller-user.component';

describe('ControllerUserComponent', () => {
  let component: ControllerUserComponent;
  let fixture: ComponentFixture<ControllerUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControllerUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControllerUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
