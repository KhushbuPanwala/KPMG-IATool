import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientParticipantsListComponent } from './client-participants-list.component';

describe('ClientParticipantsListComponent', () => {
  let component: ClientParticipantsListComponent;
  let fixture: ComponentFixture<ClientParticipantsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientParticipantsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientParticipantsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
