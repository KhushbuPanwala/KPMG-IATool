import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategicAnalysisUserSurveySecondComponent } from './strategic-analysis-user-survey-second.component';

describe('StrategicAnalysisUserSurveySecondComponent', () => {
  let component: StrategicAnalysisUserSurveySecondComponent;
  let fixture: ComponentFixture<StrategicAnalysisUserSurveySecondComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StrategicAnalysisUserSurveySecondComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StrategicAnalysisUserSurveySecondComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
