import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditableEntityCategoryAddComponent } from './auditable-entity-category-add.component';

describe('AuditableEntityCategoryAddComponent', () => {
  let component: AuditableEntityCategoryAddComponent;
  let fixture: ComponentFixture<AuditableEntityCategoryAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditableEntityCategoryAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditableEntityCategoryAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
