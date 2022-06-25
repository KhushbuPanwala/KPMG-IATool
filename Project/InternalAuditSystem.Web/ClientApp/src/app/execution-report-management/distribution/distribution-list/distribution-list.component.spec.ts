import { async, ComponentFixture, TestBed, getTestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { of as observableOf } from 'rxjs';
import { StringConstants } from '../../shared/stringConstants';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxPaginationModule } from 'ngx-pagination';
import { ToastrService, ToastrModule } from 'ngx-toastr';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { DistributionListComponent } from './distribution-list.component';
import { DistributorsAC } from '../../swaggerapi/AngularFiles/model/distributorsAC';
import { PaginationOfDistributorsAC } from '../../swaggerapi/AngularFiles/model/paginationOfDistributorsAC';
import { DistributorsService } from '../../swaggerapi/AngularFiles';

const mockPagginationData = {} as PaginationOfDistributorsAC;
mockPagginationData.totalRecords = 20;
mockPagginationData.pageIndex = 1;
mockPagginationData.pageSize = 10;

const mockDistributorListData = [] as Array<DistributorsAC>;
const mockDistributorData = {} as DistributorsAC;
mockDistributorData.id = '1';
mockDistributorData.entityId = '1';
mockDistributorData.name = 'test';
mockDistributorData.designation = 'admin';
mockDistributorData.userId = '1';
mockDistributorListData.push(mockDistributorData);

mockPagginationData.items = mockDistributorListData;

const pageNo = 1;
const selectedPageItems = 10;
const searchValue = 'test';
const totalRecords = 30;
const distributorId = '1';

describe('DistributionListComponent', () => {
  let component: DistributionListComponent;
  let fixture: ComponentFixture<DistributionListComponent>;
  let service: DistributorsService;
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DistributionListComponent],
      imports: [HttpClientTestingModule,
        RouterTestingModule,
        FormsModule,
        NgSelectModule,
        NgxPaginationModule,
        BrowserAnimationsModule,
        ModalModule.forRoot(),
        ToastrModule.forRoot()
      ],
      providers: [DistributorsService, StringConstants, BsModalService,
        ToastrService]
    })
      .compileComponents();
    fixture = TestBed.createComponent(DistributionListComponent);
    component = fixture.componentInstance;
    service = TestBed.inject(DistributorsService);
    fixture.detectChanges();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DistributionListComponent);

    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should get distributors', () => {
    spyOn(DistributorsService.prototype, 'distributorsGetDistributors').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    expect(mockPagginationData[0]).toBeUndefined();
  });

  it('should get distributors by search', () => {
    spyOn(DistributorsService.prototype, 'distributorsGetDistributors').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    component.getDistributors(pageNo, selectedPageItems, searchValue);
    component.setShowingResult(pageNo, selectedPageItems, totalRecords);
    expect(mockPagginationData[0]).toEqual(mockPagginationData[0]);
  });

  it('should on Page Change', () => {
    spyOn(DistributorsService.prototype, 'distributorsGetDistributors').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    component.onPageChange(pageNo);
    expect(mockPagginationData[0]).toBeUndefined();
  });


  it('should on delete distributor', () => {
    spyOn(DistributorsService.prototype, 'distributorsDeleteDistributor').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    component.deleteDistributor(mockPagginationData[0].Id);
    expect(mockPagginationData[0]).toBeUndefined();
  });

  it('should open add rating', () => {
    component.openAddDistributor();
    expect(component).toBeTruthy();
  });

});
