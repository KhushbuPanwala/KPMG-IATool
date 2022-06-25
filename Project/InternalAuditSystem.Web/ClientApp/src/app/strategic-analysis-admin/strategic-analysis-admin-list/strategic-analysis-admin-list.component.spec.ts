import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategicAnalysisAdminListComponent } from './strategic-analysis-admin-list.component';

describe('StrategicAnalysisAdminListComponent', () => {
  let component: StrategicAnalysisAdminListComponent;
  let fixture: ComponentFixture<StrategicAnalysisAdminListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [StrategicAnalysisAdminListComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StrategicAnalysisAdminListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
