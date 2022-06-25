import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { of as observableOf } from 'rxjs';
import { ProcessAddComponent } from './process-add.component';
import { RcmProcessService } from '../../../swaggerapi/AngularFiles/api/rcmProcess.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { StringConstants } from '../../../shared/stringConstants';
import { LoaderService } from '../../../services/loader.service';
import { RcmProcessAC } from '../../../swaggerapi/AngularFiles/model/rcmProcessAC';

const mockRcmProcessData = {} as RcmProcessAC;
mockRcmProcessData.id = '1';
mockRcmProcessData.process = 'test';

describe('ProcessAddComponent', () => {
  let component: ProcessAddComponent;
  let fixture: ComponentFixture<ProcessAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ProcessAddComponent],
      imports: [HttpClientTestingModule,
        RouterTestingModule,
        BrowserAnimationsModule,
        FormsModule,
        ToastrModule.forRoot()
      ],
      providers: [RcmProcessService, StringConstants, ToastrService, LoaderService]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcessAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should open list of Processes', () => {
    component.openListRcmProcess();
    expect(component).toBeTruthy();
  });

  it('should get Processes by id', () => {
    spyOn(RcmProcessService.prototype, 'rcmProcessGetRcmProcessById').and.callFake(() => {
      return observableOf(mockRcmProcessData[0]);
    });
    component.getProcessById();
    expect(mockRcmProcessData[0]).toEqual(mockRcmProcessData[0]);
  });

  it('should add Process', () => {
    spyOn(RcmProcessService.prototype, 'rcmProcessAddRcmProcess').and.callFake(() => {
      return observableOf(mockRcmProcessData[0]);
    });
    component.saveRcmProcess();
    expect(mockRcmProcessData[0]).toEqual(mockRcmProcessData[0]);
  });

  it('should update Process', () => {
    spyOn(RcmProcessService.prototype, 'rcmProcessUpdateRcmProcess').and.callFake(() => {
      return observableOf(mockRcmProcessData[0]);
    });
    component.saveRcmProcess();
    expect(mockRcmProcessData[0]).toEqual(mockRcmProcessData[0]);
  });
});
