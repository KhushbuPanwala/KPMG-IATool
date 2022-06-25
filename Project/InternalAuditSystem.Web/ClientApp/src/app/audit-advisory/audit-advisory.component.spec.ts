import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditAdvisoryComponent } from './audit-advisory.component';

describe('AuditAdvisoryComponent', () => {
  let component: AuditAdvisoryComponent;
  let fixture: ComponentFixture<AuditAdvisoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditAdvisoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditAdvisoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
