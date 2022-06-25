import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddTableDialogComponent } from './add-table-dialog.component';

describe('AddTableDialogComponent', () => {
  let component: AddTableDialogComponent;
  let fixture: ComponentFixture<AddTableDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddTableDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddTableDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
