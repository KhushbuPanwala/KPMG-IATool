import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategicAnalysisAdminResponseModalComponent } from './strategic-analysis-admin-response-modal.component';

describe('StrategicAnalysisAdminResponseModalComponent', () => {
  let component: StrategicAnalysisAdminResponseModalComponent;
  let fixture: ComponentFixture<StrategicAnalysisAdminResponseModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StrategicAnalysisAdminResponseModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StrategicAnalysisAdminResponseModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
