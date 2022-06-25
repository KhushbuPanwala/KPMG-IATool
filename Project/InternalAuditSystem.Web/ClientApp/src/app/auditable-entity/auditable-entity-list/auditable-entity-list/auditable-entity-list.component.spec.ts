import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditableEntityListComponent } from './auditable-entity-list.component';

describe('AuditableEntityListComponent', () => {
  let component: AuditableEntityListComponent;
  let fixture: ComponentFixture<AuditableEntityListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditableEntityListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditableEntityListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
