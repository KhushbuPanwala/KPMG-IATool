import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AcmAddComponent } from './acm-add.component';

describe('AcmAddComponent', () => {
  let component: AcmAddComponent;
  let fixture: ComponentFixture<AcmAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcmAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcmAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
