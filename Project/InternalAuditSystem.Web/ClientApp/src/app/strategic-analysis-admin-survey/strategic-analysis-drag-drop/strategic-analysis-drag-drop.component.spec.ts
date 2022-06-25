import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategicAnalysisDragDropComponent } from './strategic-analysis-drag-drop.component';

describe('StrategicAnalysisDragDropComponent', () => {
  let component: StrategicAnalysisDragDropComponent;
  let fixture: ComponentFixture<StrategicAnalysisDragDropComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StrategicAnalysisDragDropComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StrategicAnalysisDragDropComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
