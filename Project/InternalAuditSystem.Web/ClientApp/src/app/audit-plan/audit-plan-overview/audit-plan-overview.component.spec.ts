import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditPlanOverviewComponent } from './audit-plan-overview.component';

describe('AuditPlanOverviewComponent', () => {
  let component: AuditPlanOverviewComponent;
  let fixture: ComponentFixture<AuditPlanOverviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditPlanOverviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditPlanOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
