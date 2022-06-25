import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RelationshipTypeListComponent } from './relationship-type-list.component';

describe('RelationshipTypeListComponent', () => {
  let component: RelationshipTypeListComponent;
  let fixture: ComponentFixture<RelationshipTypeListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RelationshipTypeListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RelationshipTypeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
