import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportManagementCommentsComponent } from './report-management-comments.component';

describe('ReportManagementCommentsComponent ', () => {
  let component: ReportManagementCommentsComponent;
  let fixture: ComponentFixture<ReportManagementCommentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ReportManagementCommentsComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportManagementCommentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
