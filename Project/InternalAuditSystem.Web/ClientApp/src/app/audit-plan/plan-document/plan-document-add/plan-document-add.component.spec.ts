import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanDocumentAddComponent } from './plan-document-add.component';

describe('PlanDocumentAddComponent', () => {
  let component: PlanDocumentAddComponent;
  let fixture: ComponentFixture<PlanDocumentAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlanDocumentAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlanDocumentAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
