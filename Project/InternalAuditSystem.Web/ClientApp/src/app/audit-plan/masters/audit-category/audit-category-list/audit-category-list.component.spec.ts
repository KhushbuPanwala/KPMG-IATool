import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditCategoryListComponent } from './audit-category-list.component';

describe('AuditCategoryListComponent', () => {
  let component: AuditCategoryListComponent;
  let fixture: ComponentFixture<AuditCategoryListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditCategoryListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditCategoryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
