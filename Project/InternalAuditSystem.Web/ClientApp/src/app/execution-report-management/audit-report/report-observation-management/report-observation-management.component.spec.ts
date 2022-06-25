import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReportObservationManagementComponent } from './report-observation-management.component';


describe('ReportObservationManagementComponent', () => {
  let component: ReportObservationManagementComponent;
  let fixture: ComponentFixture<ReportObservationManagementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ReportObservationManagementComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportObservationManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
