import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditTeamListComponent } from './audit-team-list.component';

describe('AuditTeamListComponent', () => {
  let component: AuditTeamListComponent;
  let fixture: ComponentFixture<AuditTeamListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditTeamListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditTeamListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
