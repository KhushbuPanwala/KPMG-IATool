import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportDistributionComponent } from './report-distribution.component';

describe('ReportDistributionComponent', () => {
  let component: ReportDistributionComponent;
  let fixture: ComponentFixture<ReportDistributionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportDistributionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportDistributionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
