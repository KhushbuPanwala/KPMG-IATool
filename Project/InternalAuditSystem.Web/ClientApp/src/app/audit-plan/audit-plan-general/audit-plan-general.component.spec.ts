import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditGeneralComponent } from './audit-general.component';

describe('AuditGeneralComponent', () => {
  let component: AuditGeneralComponent;
  let fixture: ComponentFixture<AuditGeneralComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditGeneralComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditGeneralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
