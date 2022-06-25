import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RatingAddComponent } from './rating-add.component';
import { StringConstants } from '../../../../shared/stringConstants';
import { ActivatedRouteSnapshot } from '@angular/router';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { LoaderService } from '../../../../services/loader.service';
import { RatingsService, RatingAC } from '../../../../swaggerapi/AngularFiles';
import { FormsModule } from '@angular/forms';
import { of as observableOf } from 'rxjs';

const mockRatingData = {} as RatingAC;
mockRatingData.id = '1';
mockRatingData.ratings = 'test';
mockRatingData.entityId = '1';
mockRatingData.qualitativeFactors = '';
mockRatingData.quantitativeFactors = '';
mockRatingData.score = 1;
mockRatingData.legend = 'red';

const ratingId = '1';

describe('RatingAddComponent', () => {
  let component: RatingAddComponent;
  let fixture: ComponentFixture<RatingAddComponent>;
  let service: RatingsService;
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [RatingAddComponent],
      imports: [HttpClientTestingModule,
        RouterTestingModule,
        BrowserAnimationsModule,
        FormsModule,
        ToastrModule.forRoot()
      ],
      providers: [RatingsService, StringConstants, ToastrService, LoaderService]
    })
      .compileComponents();
    fixture = TestBed.createComponent(RatingAddComponent);
    component = fixture.componentInstance;
    service = TestBed.inject(RatingsService);
    fixture.detectChanges();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RatingAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should get ratings by id', () => {
    spyOn(RatingsService.prototype, 'ratingsGetRatingById').and.callFake(() => {
      return observableOf(mockRatingData[0]);
    });
    component.getRatingById();
    expect(mockRatingData[0]).toEqual(mockRatingData[0]);
  });
});
