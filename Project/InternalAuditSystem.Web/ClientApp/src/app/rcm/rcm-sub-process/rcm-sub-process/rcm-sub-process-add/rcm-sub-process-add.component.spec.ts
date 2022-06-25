import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { of as observableOf } from 'rxjs';
import { RcmSubProcessAddComponent } from './rcm-sub-process-add.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { RcmSubProcessAC, RcmSubProcessService } from '../../../../swaggerapi/AngularFiles';
import { StringConstants } from '../../../../shared/stringConstants';
import { LoaderService } from '../../../../services/loader.service';

const mockRcmSubProcessData = {} as RcmSubProcessAC;
mockRcmSubProcessData.id = '1';
mockRcmSubProcessData.subProcess = 'test';

describe('RcmSubProcessAddComponent', () => {
  let component: RcmSubProcessAddComponent;
  let fixture: ComponentFixture<RcmSubProcessAddComponent>;
  let service: RcmSubProcessService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [RcmSubProcessAddComponent],
      imports: [HttpClientTestingModule,
        RouterTestingModule,
        BrowserAnimationsModule,
        FormsModule,
        ToastrModule.forRoot()
      ],
      providers: [RcmSubProcessService, StringConstants, ToastrService, LoaderService]
    })
      .compileComponents();
    fixture = TestBed.createComponent(RcmSubProcessAddComponent);
    component = fixture.componentInstance;
    service = TestBed.inject(RcmSubProcessService);
    fixture.detectChanges();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RcmSubProcessAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should open list of SubProcesss', () => {
    component.openListRcmSubProcess();
    expect(component).toBeTruthy();
  });

  it('should get Sub Processes by id', () => {
    spyOn(RcmSubProcessService.prototype, 'rcmSubProcessGetRcmSubProcessById').and.callFake(() => {
      return observableOf(mockRcmSubProcessData[0]);
    });
    component.getSubProcessById();
    expect(mockRcmSubProcessData[0]).toEqual(mockRcmSubProcessData[0]);
  });

  it('should add Sub Process', () => {
    spyOn(RcmSubProcessService.prototype, 'rcmSubProcessAddRcmSubProcess').and.callFake(() => {
      return observableOf(mockRcmSubProcessData[0]);
    });
    component.saveRcmSubProcess();
    expect(mockRcmSubProcessData[0]).toEqual(mockRcmSubProcessData[0]);
  });

  it('should update Sub Process', () => {
    spyOn(RcmSubProcessService.prototype, 'rcmSubProcessUpdateRcmSubProcess').and.callFake(() => {
      return observableOf(mockRcmSubProcessData[0]);
    });
    component.saveRcmSubProcess();
    expect(mockRcmSubProcessData[0]).toEqual(mockRcmSubProcessData[0]);
  });
});
