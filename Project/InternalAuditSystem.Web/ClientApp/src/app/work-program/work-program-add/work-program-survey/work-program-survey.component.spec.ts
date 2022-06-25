import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkProgramSurveyComponent } from './work-program-survey.component';

describe('WorkProgramSurveyComponent', () => {
  let component: WorkProgramSurveyComponent;
  let fixture: ComponentFixture<WorkProgramSurveyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkProgramSurveyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkProgramSurveyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
