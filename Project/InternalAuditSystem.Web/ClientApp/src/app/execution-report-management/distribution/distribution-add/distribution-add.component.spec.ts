import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DistributionAddComponent } from './distribution-add.component';

describe('DistributionAddComponent', () => {
  let component: DistributionAddComponent;
  let fixture: ComponentFixture<DistributionAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DistributionAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DistributionAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
