import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadReportFileDialogComponent } from './upload-report-file-dialog.component';

describe('UploadReportFileDialogComponent', () => {
  let component: UploadReportFileDialogComponent;
  let fixture: ComponentFixture<UploadReportFileDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [UploadReportFileDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadReportFileDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
