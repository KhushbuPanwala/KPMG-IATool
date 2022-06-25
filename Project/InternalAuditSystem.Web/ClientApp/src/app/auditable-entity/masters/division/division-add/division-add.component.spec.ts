import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DivisionAddComponent } from './division-add.component';

describe('DivisionAddComponent', () => {
  let component: DivisionAddComponent;
  let fixture: ComponentFixture<DivisionAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DivisionAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DivisionAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
