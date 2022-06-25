import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerateAcmReportComponent } from './generate-acm-report.component';

describe('GenerateAcmReportComponent', () => {
  let component: GenerateAcmReportComponent;
  let fixture: ComponentFixture<GenerateAcmReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenerateAcmReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerateAcmReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
