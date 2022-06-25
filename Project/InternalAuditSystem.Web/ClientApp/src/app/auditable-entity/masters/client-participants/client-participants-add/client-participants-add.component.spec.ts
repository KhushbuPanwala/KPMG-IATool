import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientParticipantsAddComponent } from './client-participants-add.component';

describe('ClientParticipantsAddComponent', () => {
  let component: ClientParticipantsAddComponent;
  let fixture: ComponentFixture<ClientParticipantsAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientParticipantsAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientParticipantsAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
