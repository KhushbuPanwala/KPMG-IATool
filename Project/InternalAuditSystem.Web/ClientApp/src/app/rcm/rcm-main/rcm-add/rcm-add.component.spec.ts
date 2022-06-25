import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RcmAddComponent } from './rcm-add.component';

describe('RcmAddComponent', () => {
  let component: RcmAddComponent;
  let fixture: ComponentFixture<RcmAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [RcmAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RcmAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
