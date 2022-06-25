import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../../shared/stringConstants';
import { ActivatedRoute, Router } from '@angular/router';
import { RatingAC } from '../../../../swaggerapi/AngularFiles/model/ratingAC';
import { RatingsService } from '../../../../swaggerapi/AngularFiles';
import { SharedService } from '../../../../core/shared.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-rating-add',
  templateUrl: './rating-add.component.html',
  styleUrls: ['./rating-add.component.scss']
})
export class RatingAddComponent implements OnInit, OnDestroy {
  ratingLabel: string; // Variable for rating title
  backToolTip: string; // Variable for back tooltip
  legendLabel: string; // Variable for legend label
  scoreLabel: string; // Variable for score label
  qualitativeFactorsLabel: string; // Variable for qualitative factors
  quantitativeFactorsLabel: string; // Variable for qualitative factors
  saveButtonText: string; // Variable for save button
  redColor: string; // Variable for red color
  yellowColor: string; // Variable for yellow color
  greenColor: string; // Varibale for greenColor

  ratingId: string;
  rating = {} as RatingAC;
  selectedPageItem: number;
  searchValue: string;

  invalidMessage: string;
  requiredMessage: string;
  maxLengthExceedMessage: string;
  selectedEntityId;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  // Creates an instance of documenter.
  constructor(
    private stringConstants: StringConstants, private route: ActivatedRoute, private apiService: RatingsService,
    public router: Router, private sharedService: SharedService) {
    this.ratingLabel = this.stringConstants.ratingLabel;
    this.backToolTip = this.stringConstants.backToolTip;
    this.legendLabel = this.stringConstants.legendLabel;
    this.scoreLabel = this.stringConstants.scoreLabel;
    this.qualitativeFactorsLabel = this.stringConstants.qualitativeFactors;
    this.quantitativeFactorsLabel = this.stringConstants.quantitativeFactors;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.redColor = this.stringConstants.redColor;
    this.yellowColor = this.stringConstants.yellowColor;
    this.greenColor = this.stringConstants.greenColor;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;

        this.route.params.subscribe(params => {
          this.ratingId = params.id;
          this.selectedPageItem = params.pageItems;
          this.searchValue = params.searchValue;
        });
        if (this.ratingId !== '0') {
          this.getRatingById();
        } else {
          this.rating = {} as RatingAC;
          this.rating.legend = this.redColor;
        }
      }
    });
  }

  /**
   * A lifecycle hook that is called when a directive, pipe, or service is destroyed.
   * Use for any custom cleanup that needs to occur when the instance is destroyed.
   */
  ngOnDestroy() {
    this.entitySubscribe.unsubscribe();
  }

  /**
   * Get Rating detail by id for edit
   */
  getRatingById() {
    this.apiService.ratingsGetRatingById(this.ratingId, this.selectedEntityId).subscribe(result => {
      this.rating = result;
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }
  /**
   * Add and update Rating
   */
  saveRating() {
    this.rating.entityId = this.selectedEntityId;
    if (this.rating.id === undefined) {
      this.apiService.ratingsAddRating(this.rating, this.selectedEntityId).subscribe(result => {
        this.rating = result;
        this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
        this.setListPageRoute();
      }, (error) => {
        this.sharedService.handleError(error);
      });
    } else {
      this.apiService.ratingsUpdateRating(this.rating, this.selectedEntityId).subscribe(result => {
        this.rating = result;
        this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
        this.setListPageRoute();
      }, (error) => {
        this.sharedService.handleError(error);
      });
    }
  }
  /**
   * on back button route to list page
   */
  onBackClick() {
    this.setListPageRoute();
  }
  /**
   * set route for list page redirection
   */
  setListPageRoute() {
    this.router.navigate(['rating/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

}
