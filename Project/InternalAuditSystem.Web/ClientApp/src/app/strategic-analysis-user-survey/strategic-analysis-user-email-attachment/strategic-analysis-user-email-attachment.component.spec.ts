import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategicAnalysisUserEmailAttachmentComponent } from './strategic-analysis-user-email-attachment.component';

describe('StrategicAnalysisUserEmailAttachmentComponent', () => {
  let component: StrategicAnalysisUserEmailAttachmentComponent;
  let fixture: ComponentFixture<StrategicAnalysisUserEmailAttachmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StrategicAnalysisUserEmailAttachmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StrategicAnalysisUserEmailAttachmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
