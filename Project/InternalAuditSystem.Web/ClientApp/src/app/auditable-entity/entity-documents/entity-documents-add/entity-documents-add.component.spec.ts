import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EntityDocumentsAddComponent } from './entity-documents-add.component';

describe('EntityDocumentsAddComponent', () => {
  let component: EntityDocumentsAddComponent;
  let fixture: ComponentFixture<EntityDocumentsAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EntityDocumentsAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EntityDocumentsAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
