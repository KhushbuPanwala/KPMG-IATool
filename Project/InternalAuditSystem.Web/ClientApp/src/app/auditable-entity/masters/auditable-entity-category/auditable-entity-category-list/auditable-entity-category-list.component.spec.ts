import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditableEntityCategoryListComponent } from './auditable-entity-category-list.component';

describe('AuditableEntityCategoryListComponent', () => {
  let component: AuditableEntityCategoryListComponent;
  let fixture: ComponentFixture<AuditableEntityCategoryListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditableEntityCategoryListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditableEntityCategoryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
