import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditTypeListComponent } from './audit-type-list.component';

describe('AuditTypeListComponent', () => {
  let component: AuditTypeListComponent;
  let fixture: ComponentFixture<AuditTypeListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AuditTypeListComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditTypeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
