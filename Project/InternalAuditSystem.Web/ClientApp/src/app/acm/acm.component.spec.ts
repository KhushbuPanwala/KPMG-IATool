import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AcmComponent } from './acm.component';

describe('AcmComponent', () => {
  let component: AcmComponent;
  let fixture: ComponentFixture<AcmComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcmComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
