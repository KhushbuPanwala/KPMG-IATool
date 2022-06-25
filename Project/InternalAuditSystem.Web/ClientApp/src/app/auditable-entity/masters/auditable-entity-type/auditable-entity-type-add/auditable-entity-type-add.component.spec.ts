import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditableEntityTypeAddComponent } from './auditable-entity-type-add.component';

describe('AuditableEntityTypeAddComponent', () => {
  let component: AuditableEntityTypeAddComponent;
  let fixture: ComponentFixture<AuditableEntityTypeAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditableEntityTypeAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditableEntityTypeAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
