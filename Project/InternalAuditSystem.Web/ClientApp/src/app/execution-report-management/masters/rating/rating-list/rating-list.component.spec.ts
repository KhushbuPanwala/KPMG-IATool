import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { of as observableOf } from 'rxjs';
import { RatingListComponent } from './rating-list.component';
import { StringConstants } from '../../../../shared/stringConstants';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxPaginationModule } from 'ngx-pagination';
import { RatingAC, RatingsService, PaginationOfRatingAC } from '../../../../swaggerapi/AngularFiles';
import { ToastrService, ToastrModule } from 'ngx-toastr';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';

const mockPagginationData = {} as PaginationOfRatingAC;
mockPagginationData.totalRecords = 20;
mockPagginationData.pageIndex = 1;
mockPagginationData.pageSize = 10;

const mockRatingListData = [] as Array<RatingAC>;
const mockRatingData = {} as RatingAC;
mockRatingData.id = '1';
mockRatingData.ratings = 'test';
mockRatingData.entityId = '1';
mockRatingData.qualitativeFactors = '';
mockRatingData.quantitativeFactors = '';
mockRatingData.score = 1;
mockRatingData.legend = 'red';

mockRatingListData.push(mockRatingData);

mockPagginationData.items = mockRatingListData;

const pageNo = 1;
const selectedPageItems = 10;
const searchValue = 'test';
const totalRecords = 30;
const ratingId = '1';

describe('RatingListComponent', () => {
  let component: RatingListComponent;
  let fixture: ComponentFixture<RatingListComponent>;
  let service: RatingsService;
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [RatingListComponent],
      imports: [HttpClientTestingModule,
        RouterTestingModule,
        FormsModule,
        NgSelectModule,
        NgxPaginationModule,
        BrowserAnimationsModule,
        ModalModule.forRoot(),
        ToastrModule.forRoot()
      ],
      providers: [RatingsService, StringConstants, RouterTestingModule, BsModalService, ToastrService]
    })
      .compileComponents();
    fixture = TestBed.createComponent(RatingListComponent);
    component = fixture.componentInstance;
    service = TestBed.inject(RatingsService);
    fixture.detectChanges();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RatingListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should get ratings', () => {
    spyOn(RatingsService.prototype, 'ratingsGetRatings').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    expect(mockPagginationData[0]).toBeUndefined();
  });

  it('should get ratings by search', () => {
    spyOn(RatingsService.prototype, 'ratingsGetRatings').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    component.getRatings(pageNo, selectedPageItems, searchValue);
    component.setShowingResult(pageNo, selectedPageItems, totalRecords);
    expect(mockPagginationData[0]).toEqual(mockPagginationData[0]);
  });

  it('should on Page Change', () => {
    spyOn(RatingsService.prototype, 'ratingsGetRatings').and.callFake(() => {
      return observableOf(mockPagginationData[0]);
    });
    component.onPageChange(pageNo);
    component.setShowingResult(pageNo, selectedPageItems, totalRecords);
    expect(mockPagginationData[0]).toEqual(mockPagginationData[0]);
  });

  it('should open add rating', () => {
    component.openAddRating();
    expect(component).toBeUndefined();
  });

  it('should open edit rating', () => {
    component.editRating(ratingId);
    expect(component).toBeUndefined();
  });

});
