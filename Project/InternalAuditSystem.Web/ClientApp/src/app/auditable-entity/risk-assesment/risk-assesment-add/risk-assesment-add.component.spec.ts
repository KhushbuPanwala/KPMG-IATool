import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RiskAssesmentAddComponent } from './risk-assesment-add.component';

describe('RiskAssesmentAddComponent', () => {
  let component: RiskAssesmentAddComponent;
  let fixture: ComponentFixture<RiskAssesmentAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RiskAssesmentAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RiskAssesmentAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
