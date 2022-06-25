import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RelationshipTypeAddComponent } from './relationship-type-add.component';

describe('RelationshipTypeAddComponent', () => {
  let component: RelationshipTypeAddComponent;
  let fixture: ComponentFixture<RelationshipTypeAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RelationshipTypeAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RelationshipTypeAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
