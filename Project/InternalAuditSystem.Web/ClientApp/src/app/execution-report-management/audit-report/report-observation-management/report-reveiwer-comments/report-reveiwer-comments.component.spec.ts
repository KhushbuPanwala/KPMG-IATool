import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportReveiwerCommentsComponent } from './report-reveiwer-comments.component';

describe('ReportReveiwerCommentsComponent', () => {
  let component: ReportReveiwerCommentsComponent;
  let fixture: ComponentFixture<ReportReveiwerCommentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ReportReveiwerCommentsComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportReveiwerCommentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
