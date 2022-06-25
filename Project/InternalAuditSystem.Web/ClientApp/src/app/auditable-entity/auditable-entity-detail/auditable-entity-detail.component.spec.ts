import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditableEntityDetailComponent } from './auditable-entity-detail.component';

describe('AuditableEntityDetailComponent', () => {
  let component: AuditableEntityDetailComponent;
  let fixture: ComponentFixture<AuditableEntityDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditableEntityDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditableEntityDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
