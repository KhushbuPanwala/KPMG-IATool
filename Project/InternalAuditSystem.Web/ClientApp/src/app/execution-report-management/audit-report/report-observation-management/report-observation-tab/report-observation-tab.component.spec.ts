import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportObservationTabComponent } from './report-observation-tab.component';

describe('ReportObservationTabComponent', () => {
  let component: ReportObservationTabComponent;
  let fixture: ComponentFixture<ReportObservationTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ReportObservationTabComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportObservationTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
