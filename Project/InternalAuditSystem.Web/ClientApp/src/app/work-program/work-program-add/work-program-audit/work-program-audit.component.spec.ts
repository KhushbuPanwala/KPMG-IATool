import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkProgramAuditComponent } from './work-program-audit.component';

describe('WorkProgramAuditComponent', () => {
  let component: WorkProgramAuditComponent;
  let fixture: ComponentFixture<WorkProgramAuditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkProgramAuditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkProgramAuditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
