import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EntityDocumentsComponent } from './entity-documents.component';

describe('EntityDocumentsComponent', () => {
  let component: EntityDocumentsComponent;
  let fixture: ComponentFixture<EntityDocumentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EntityDocumentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EntityDocumentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
