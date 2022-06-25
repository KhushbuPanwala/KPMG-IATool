import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkProgramSamplingMethodologyComponent } from './work-program-sampling-methodology.component';

describe('WorkProgramSamplingMethodologyComponent', () => {
  let component: WorkProgramSamplingMethodologyComponent;
  let fixture: ComponentFixture<WorkProgramSamplingMethodologyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkProgramSamplingMethodologyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkProgramSamplingMethodologyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
