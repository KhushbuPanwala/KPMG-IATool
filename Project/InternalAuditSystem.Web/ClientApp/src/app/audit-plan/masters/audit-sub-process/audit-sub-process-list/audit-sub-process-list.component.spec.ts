import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditSubProcessListComponent } from './audit-sub-process-list.component';

describe('AuditProcessListComponent', () => {
  let component: AuditSubProcessListComponent;
  let fixture: ComponentFixture<AuditSubProcessListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AuditSubProcessListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditSubProcessListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
