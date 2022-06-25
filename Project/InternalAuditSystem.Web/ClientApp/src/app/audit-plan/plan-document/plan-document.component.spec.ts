import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanDocumentComponent } from './plan-document.component';

describe('PlanDocumentComponent', () => {
  let component: PlanDocumentComponent;
  let fixture: ComponentFixture<PlanDocumentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlanDocumentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlanDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
