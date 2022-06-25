import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkProgramListComponent } from './work-program-list.component';

describe('WorkProgramListComponent', () => {
  let component: WorkProgramListComponent;
  let fixture: ComponentFixture<WorkProgramListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkProgramListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkProgramListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
