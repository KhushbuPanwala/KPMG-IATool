import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GeographicalAreaAddComponent } from './geographical-area-add.component';

describe('GeographicalAreaAddComponent', () => {
  let component: GeographicalAreaAddComponent;
  let fixture: ComponentFixture<GeographicalAreaAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeographicalAreaAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeographicalAreaAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
