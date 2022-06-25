import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditTeamAddComponent } from './audit-team-add.component';

describe('AuditTeamAddComponent', () => {
  let component: AuditTeamAddComponent;
  let fixture: ComponentFixture<AuditTeamAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditTeamAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditTeamAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
