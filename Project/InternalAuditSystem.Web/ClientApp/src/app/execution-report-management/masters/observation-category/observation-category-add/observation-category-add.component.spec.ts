import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationCategoryAddComponent } from './observation-category-add.component';

describe('ObservationCategoryAddComponent', () => {
  let component: ObservationCategoryAddComponent;
  let fixture: ComponentFixture<ObservationCategoryAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservationCategoryAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationCategoryAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
