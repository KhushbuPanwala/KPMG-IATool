import { RcmListComponent } from './rcm-list.component';
import { ComponentFixture, async, TestBed } from '@angular/core/testing';

describe('RcmListComponent', () => {
  let component: RcmListComponent;
  let fixture: ComponentFixture<RcmListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [RcmListComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RcmListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
