import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditTypeAddComponent } from './audit-type-add.component';

describe('AuditTypeAddComponent', () => {
  let component: AuditTypeAddComponent;
  let fixture: ComponentFixture<AuditTypeAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditTypeAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditTypeAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
