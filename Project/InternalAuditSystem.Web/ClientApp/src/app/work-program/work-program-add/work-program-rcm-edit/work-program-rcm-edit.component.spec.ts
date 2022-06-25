import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkProgramRcmEditComponent } from './work-program-rcm-edit.component';

describe('WorkProgramRcmEditComponent', () => {
  let component: WorkProgramRcmEditComponent;
  let fixture: ComponentFixture<WorkProgramRcmEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkProgramRcmEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkProgramRcmEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
