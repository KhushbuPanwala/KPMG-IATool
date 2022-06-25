import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanAuditProcessComponent } from './plan-audit-process.component';

describe('PlanAuditProcessComponent', () => {
  let component: PlanAuditProcessComponent;
  let fixture: ComponentFixture<PlanAuditProcessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlanAuditProcessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlanAuditProcessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
