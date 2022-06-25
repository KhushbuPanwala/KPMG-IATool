import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkProgramAddComponent } from './work-program-add.component';

describe('WorkProgramAddComponent', () => {
  let component: WorkProgramAddComponent;
  let fixture: ComponentFixture<WorkProgramAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkProgramAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkProgramAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
