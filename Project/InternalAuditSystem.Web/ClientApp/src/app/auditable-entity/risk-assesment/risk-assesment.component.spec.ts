import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RiskAssesmentComponent } from './risk-assesment.component';

describe('RiskAssesmentComponent', () => {
  let component: RiskAssesmentComponent;
  let fixture: ComponentFixture<RiskAssesmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RiskAssesmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RiskAssesmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
