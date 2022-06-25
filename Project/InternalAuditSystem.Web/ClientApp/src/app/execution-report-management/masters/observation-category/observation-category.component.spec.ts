import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationCategoryComponent } from './observation-category.component';

describe('ObservationCategoryComponent', () => {
  let component: ObservationCategoryComponent;
  let fixture: ComponentFixture<ObservationCategoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservationCategoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
