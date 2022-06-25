import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkProgramRcmComponent } from './work-program-rcm.component';

describe('WorkProgramRcmComponent', () => {
  let component: WorkProgramRcmComponent;
  let fixture: ComponentFixture<WorkProgramRcmComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkProgramRcmComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkProgramRcmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
