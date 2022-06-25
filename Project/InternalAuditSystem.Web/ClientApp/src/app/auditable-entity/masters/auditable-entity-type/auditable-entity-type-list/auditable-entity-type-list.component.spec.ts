import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditableEntityTypeListComponent } from './auditable-entity-type-list.component';

describe('AuditableEntityTypeListComponent', () => {
  let component: AuditableEntityTypeListComponent;
  let fixture: ComponentFixture<AuditableEntityTypeListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditableEntityTypeListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditableEntityTypeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
