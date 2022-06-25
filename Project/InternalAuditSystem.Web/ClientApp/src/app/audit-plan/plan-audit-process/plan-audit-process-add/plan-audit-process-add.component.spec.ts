import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanAuditProcessAddComponent } from './plan-audit-process-add.component';

describe('PlanAuditProcessAddComponent', () => {
  let component: PlanAuditProcessAddComponent;
  let fixture: ComponentFixture<PlanAuditProcessAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlanAuditProcessAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlanAuditProcessAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
