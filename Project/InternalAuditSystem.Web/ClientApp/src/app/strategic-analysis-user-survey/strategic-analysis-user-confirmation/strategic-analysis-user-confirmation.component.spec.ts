import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategicAnalysisUserConfirmationComponent } from './strategic-analysis-user-confirmation.component';

describe('StrategicAnalysisUserConfirmationComponent', () => {
  let component: StrategicAnalysisUserConfirmationComponent;
  let fixture: ComponentFixture<StrategicAnalysisUserConfirmationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StrategicAnalysisUserConfirmationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StrategicAnalysisUserConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
