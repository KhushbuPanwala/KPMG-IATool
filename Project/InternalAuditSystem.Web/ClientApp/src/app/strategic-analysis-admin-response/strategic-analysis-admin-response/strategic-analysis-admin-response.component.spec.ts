import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategicAnalysisAdminResponseComponent } from './strategic-analysis-admin-response.component';

describe('StrategicAnalysisAdminResponseComponent', () => {
  let component: StrategicAnalysisAdminResponseComponent;
  let fixture: ComponentFixture<StrategicAnalysisAdminResponseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StrategicAnalysisAdminResponseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StrategicAnalysisAdminResponseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
