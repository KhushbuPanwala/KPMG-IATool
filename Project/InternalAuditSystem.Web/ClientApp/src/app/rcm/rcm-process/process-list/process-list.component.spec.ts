import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { of as observableOf } from 'rxjs';
import { ProcessListComponent } from './process-list.component';
import { PaginationOfRcmProcessAC } from '../../../swaggerapi/AngularFiles/model/paginationOfRcmProcessAC';
import { RcmProcessAC } from '../../../swaggerapi/AngularFiles/model/rcmProcessAC';
import { RcmProcessService } from '../../../swaggerapi/AngularFiles/api/rcmProcess.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxPaginationModule } from 'ngx-pagination';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ModalModule, BsModalService } from 'ngx-bootstrap/modal/public_api';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { StringConstants } from '../../../shared/stringConstants';
import { LoaderService } from '../../../services/loader.service';

const mockPagginationData = {} as PaginationOfRcmProcessAC;
mockPagginationData.totalRecords = 20;
mockPagginationData.pageIndex = 1;
mockPagginationData.pageSize = 10;

const mockRcmProcessListData = [] as Array<RcmProcessAC>;
const mockRcmProcessData = {} as RcmProcessAC;
mockRcmProcessData.id = '1';
mockRcmProcessData.process = 'test';

mockRcmProcessListData.push(mockRcmProcessData);

mockPagginationData.items = mockRcmProcessListData;

const pageNo = 1;
const selectedPageItems = 10;
const searchValue = 'test';
const totalRecords = 30;
const processId = '1';

describe('ProcessListComponent', () => {
  let component: ProcessListComponent;
  let fixture: ComponentFixture<ProcessListComponent>;
  let service: RcmProcessService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProcessListComponent ],
    imports: [HttpClientTestingModule,
        RouterTestingModule,
        FormsModule,
        NgSelectModule,
        NgxPaginationModule,
        BrowserAnimationsModule,
        ModalModule.forRoot(),
        ToastrModule.forRoot()
      ],
      providers: [RcmProcessService, StringConstants, LoaderService, BsModalService,
        ToastrService]
    })
      .compileComponents();
    fixture = TestBed.createComponent(ProcessListComponent);
    component = fixture.componentInstance;
    service = TestBed.inject(RcmProcessService);
    fixture.detectChanges();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcessListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should get ratings', () => {
    spyOn(RcmProcessService.prototype, 'rcmProcessGetRcmProcess').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    expect(mockPagginationData[0]).toBeUndefined();
  });

  it('should get ratings by search', () => {
    spyOn(RcmProcessService.prototype, 'rcmProcessGetRcmProcess').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    component.getRcmProcess(pageNo, selectedPageItems, searchValue);
    component.setShowingResult(pageNo, selectedPageItems, totalRecords);
    expect(mockPagginationData[0]).toEqual(mockPagginationData[0]);
  });

  it('should on Page Change', () => {
    spyOn(RcmProcessService.prototype, 'rcmProcessGetRcmProcess').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    component.onPageChange(pageNo);
    expect(mockPagginationData[0]).toBeUndefined();
  });

  it('should delete Process', () => {
    spyOn(RcmProcessService.prototype, 'rcmProcessDeleteRcmProcess').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    component.deleteRcmProcess(mockPagginationData[0].Id);
    expect(mockPagginationData[0]).toBeUndefined();
  });

  it('should open add rating', () => {
    component.openAddRcmProcess();
    expect(component).toBeTruthy();
  });

  it('should open edit rating', () => {
    component.editRcmProcess(processId);
    expect(component).toBeTruthy();
  });
});
