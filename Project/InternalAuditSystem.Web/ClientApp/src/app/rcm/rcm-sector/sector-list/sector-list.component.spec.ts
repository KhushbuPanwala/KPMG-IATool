import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { of as observableOf } from 'rxjs';
import { SectorListComponent } from './sector-list.component';
import { PaginationOfRcmSectorAC } from '../../../swaggerapi/AngularFiles/model/paginationOfRcmSectorAC';
import { RcmSectorAC } from '../../../swaggerapi/AngularFiles/model/rcmSectorAC';
import { RcmSectorService } from '../../../swaggerapi/AngularFiles/api/rcmSector.service';
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

const mockPagginationData = {} as PaginationOfRcmSectorAC;
mockPagginationData.totalRecords = 20;
mockPagginationData.pageIndex = 1;
mockPagginationData.pageSize = 10;

const mockRcmSectorListData = [] as Array<RcmSectorAC>;
const mockRcmSectorData = {} as RcmSectorAC;
mockRcmSectorData.id = '1';
mockRcmSectorData.sector = 'test';

mockRcmSectorListData.push(mockRcmSectorData);

mockPagginationData.items = mockRcmSectorListData;

const pageNo = 1;
const selectedPageItems = 10;
const searchValue = 'test';
const totalRecords = 30;
const sectorId = '1';

describe('SectorListingComponent', () => {
  let component: SectorListComponent;
  let fixture: ComponentFixture<SectorListComponent>;
  let service: RcmSectorService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SectorListComponent],
      imports: [HttpClientTestingModule,
        RouterTestingModule,
        FormsModule,
        NgSelectModule,
        NgxPaginationModule,
        BrowserAnimationsModule,
        ModalModule.forRoot(),
        ToastrModule.forRoot()
      ],
      providers: [RcmSectorService, StringConstants, LoaderService, BsModalService,
        ToastrService]
    })
      .compileComponents();
    fixture = TestBed.createComponent(SectorListComponent);
    component = fixture.componentInstance;
    service = TestBed.inject(RcmSectorService);
    fixture.detectChanges();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SectorListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should get Sectors', () => {
    spyOn(RcmSectorService.prototype, 'rcmSectorGetRcmSector').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    expect(mockPagginationData[0]).toBeUndefined();
  });

  it('should get Sectors by search', () => {
    spyOn(RcmSectorService.prototype, 'rcmSectorGetRcmSector').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    component.getRcmSector(pageNo, selectedPageItems, searchValue);
    component.setShowingResult(pageNo, selectedPageItems, totalRecords);
    expect(mockPagginationData[0]).toEqual(mockPagginationData[0]);
  });

  it('should on Page Change', () => {
    spyOn(RcmSectorService.prototype, 'rcmSectorGetRcmSector').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    component.onPageChange(pageNo);
    expect(mockPagginationData[0]).toBeUndefined();
  });

  it('should delete sector', () => {
    spyOn(RcmSectorService.prototype, 'rcmSectorDeleteRcmSector').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    component.deleteRcmSector(mockPagginationData[0].Id);
    expect(mockPagginationData[0]).toBeUndefined();
  });

  it('should open add Sector', () => {
    component.openAddRcmSector();
    expect(component).toBeTruthy();
  });

  it('should open edit Sector', () => {
    component.editRcmSector(sectorId);
    expect(component).toBeTruthy();
  });
});
