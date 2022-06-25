import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategicAnalysisUserComponent } from './strategic-analysis-user.component';

describe('StrategicAnalysisUserComponent', () => {
  let component: StrategicAnalysisUserComponent;
  let fixture: ComponentFixture<StrategicAnalysisUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StrategicAnalysisUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StrategicAnalysisUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
