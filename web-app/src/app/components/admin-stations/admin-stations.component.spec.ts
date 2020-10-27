import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminStationsComponent } from './admin-stations.component';

describe('AdminStationsComponent', () => {
  let component: AdminStationsComponent;
  let fixture: ComponentFixture<AdminStationsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminStationsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminStationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
