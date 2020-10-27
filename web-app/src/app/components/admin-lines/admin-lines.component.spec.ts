import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminLinesComponent } from './admin-lines.component';

describe('AdminLinesComponent', () => {
  let component: AdminLinesComponent;
  let fixture: ComponentFixture<AdminLinesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminLinesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminLinesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
