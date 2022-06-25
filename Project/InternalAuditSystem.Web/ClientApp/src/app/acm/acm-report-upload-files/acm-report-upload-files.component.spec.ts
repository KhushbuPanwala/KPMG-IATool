import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AcmReportUploadFilesComponent } from './acm-report-upload-files.component';

describe('AcmReportUploadFilesComponent', () => {
  let component: AcmReportUploadFilesComponent;
  let fixture: ComponentFixture<AcmReportUploadFilesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcmReportUploadFilesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcmReportUploadFilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
