import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategicAnalysisAdminSurveyComponent } from './strategic-analysis-admin-survey.component';

describe('StrategicAnalysisAdminSurveyComponent', () => {
  let component: StrategicAnalysisAdminSurveyComponent;
  let fixture: ComponentFixture<StrategicAnalysisAdminSurveyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StrategicAnalysisAdminSurveyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StrategicAnalysisAdminSurveyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
