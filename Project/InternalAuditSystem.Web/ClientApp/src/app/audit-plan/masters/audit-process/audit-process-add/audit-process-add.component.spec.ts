import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditProcessAddComponent } from './audit-process-add.component';

describe('AuditProcessAddComponent', () => {
  let component: AuditProcessAddComponent;
  let fixture: ComponentFixture<AuditProcessAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditProcessAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditProcessAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
