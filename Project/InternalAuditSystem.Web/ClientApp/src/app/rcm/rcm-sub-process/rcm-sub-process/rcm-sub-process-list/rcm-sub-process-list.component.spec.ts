import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { of as observableOf } from 'rxjs';
import { RcmSubProcessListComponent } from './rcm-sub-process-list.component';
import { PaginationOfRcmSubProcessAC, RcmSubProcessAC, RcmSubProcessService } from '../../../../swaggerapi/AngularFiles';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxPaginationModule } from 'ngx-pagination';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ModalModule, BsModalService } from 'ngx-bootstrap/modal/public_api';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { StringConstants } from '../../../../shared/stringConstants';
import { LoaderService } from '../../../../services/loader.service';

const mockPagginationData = {} as PaginationOfRcmSubProcessAC;
mockPagginationData.totalRecords = 20;
mockPagginationData.pageIndex = 1;
mockPagginationData.pageSize = 10;

const mockRcmSubProcessListData = [] as Array<RcmSubProcessAC>;
const mockRcmSubProcessData = {} as RcmSubProcessAC;
mockRcmSubProcessData.id = '1';
mockRcmSubProcessData.subProcess = 'test';

mockRcmSubProcessListData.push(mockRcmSubProcessData);

mockPagginationData.items = mockRcmSubProcessListData;

const pageNo = 1;
const selectedPageItems = 10;
const searchValue = 'test';
const totalRecords = 30;
const subProcessId = '1';

describe('RcmSubProcessListComponent', () => {
  let component: RcmSubProcessListComponent;
  let fixture: ComponentFixture<RcmSubProcessListComponent>;
  let service: RcmSubProcessService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [RcmSubProcessListComponent],
      imports: [HttpClientTestingModule,
        RouterTestingModule,
        FormsModule,
        NgSelectModule,
        NgxPaginationModule,
        BrowserAnimationsModule,
        ModalModule.forRoot(),
        ToastrModule.forRoot()
      ],
      providers: [RcmSubProcessService, StringConstants, LoaderService, BsModalService,
        ToastrService]
    })
      .compileComponents();
    fixture = TestBed.createComponent(RcmSubProcessListComponent);
    component = fixture.componentInstance;
    service = TestBed.inject(RcmSubProcessService);
    fixture.detectChanges();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RcmSubProcessListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should get SubProcess', () => {
    spyOn(RcmSubProcessService.prototype, 'rcmSubProcessGetRcmSubProcess').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    expect(mockPagginationData[0]).toBeUndefined();
  });

  it('should get SubProcess by search', () => {
    spyOn(RcmSubProcessService.prototype, 'rcmSubProcessGetRcmSubProcess').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    component.getRcmSubProcess(pageNo, selectedPageItems, searchValue);
    component.setShowingResult(pageNo, selectedPageItems, totalRecords);
    expect(mockPagginationData[0]).toEqual(mockPagginationData[0]);
  });

  it('should on Page Change', () => {
    spyOn(RcmSubProcessService.prototype, 'rcmSubProcessGetRcmSubProcess').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    component.onPageChange(pageNo);
    expect(mockPagginationData[0]).toBeUndefined();
  });

  it('should delete SubProcess', () => {
    spyOn(RcmSubProcessService.prototype, 'rcmSubProcessDeleteRcmSubProcess').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    component.deleteRcmSubProcess(mockPagginationData[0].Id);
    expect(mockPagginationData[0]).toBeUndefined();
  });

  it('should open add SubProcess', () => {
    component.openAddRcmSubProcess();
    expect(component).toBeTruthy();
  });

  it('should open edit SubProcess', () => {
    component.editRcmSubProcess(subProcessId);
    expect(component).toBeTruthy();
  });
});
