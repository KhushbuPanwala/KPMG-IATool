import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategicAnalysisUserSurveyComponent } from './strategic-analysis-user-survey.component';

describe('StrategicAnalysisUserSurveyComponent', () => {
  let component: StrategicAnalysisUserSurveyComponent;
  let fixture: ComponentFixture<StrategicAnalysisUserSurveyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StrategicAnalysisUserSurveyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StrategicAnalysisUserSurveyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
