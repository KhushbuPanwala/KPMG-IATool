import { Component, OnInit } from '@angular/core';
import { StringConstants } from '../../../../shared/stringConstants';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { SharedService } from '../../../../core/shared.service';
import { EntityCountryAC, CountriesService, CountryAC, RegionAC } from '../../../../swaggerapi/AngularFiles';
import { LoaderService } from '../../../../core/loader.service';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';


@Component({
  selector: 'app-country-add',
  templateUrl: './country-add.component.html',
})
export class CountryAddComponent implements OnInit {

  regionText: string; // Variable for region text
  countryText: string; // Variable for country text
  saveButtonText: string; // Variable for save button text
  selected: string; // Variable for selected typeahead
  selectedEntityId: string;
  countryObject = {} as EntityCountryAC;
  entityCountryId: string;
  countryId: string;
  regionId: string;
  countryList: string[] = [];
  getCountryList: CountryAC[] = [];
  tempCountryList: CountryAC[] = [];
  tempRegionList: RegionAC[] = [];
  isLoading = false;
  isToHideList = true;
  isDataPopulated = false;
  isNoRecordMatched: boolean;
  displayNameList = [] as Array<string>;
  dropdownList: Observable<CountryAC[]>;
  subject = new Subject<string>();
  requiredMessage: string;
  noRecordFound: string;
  maxLengthExceedMessage: string;
  invalidMessage: string;

  // Creates an instance of documenter
  constructor(private stringConstants: StringConstants,
              public bsModalRef: BsModalRef, private sharedService: SharedService,
              private countryService: CountriesService, private loaderService: LoaderService, private route: ActivatedRoute) {
    this.regionText = this.stringConstants.regionText;
    this.countryText = this.stringConstants.countryText;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.countryId = '';
    this.regionId = '';
    this.requiredMessage = this.stringConstants.countryRequiredMessage;
    this.noRecordFound = this.stringConstants.noRecordFoundMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.invalidMessage = this.stringConstants.invalidMessage;
  }




  /** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      this.selectedEntityId = entityId;
      this.entityCountryId = this.countryObject.id;
      this.getCountryById(this.entityCountryId, this.selectedEntityId);
      // get all users from ad based on seraching
      this.getCountryBasedOnSearch();

    });
  }

  /**
   * Method for getting country data by id
   * @param entityCountryId : Id of entityCountry
   * @param entityId : Id of selected entity
   */
  getCountryById(entityCountryId: string, entityId: string) {
    this.countryService.countriesGetCountryById(entityCountryId, entityId).subscribe(result => {
      this.countryObject.regionACList = result.regionACList;
      this.tempRegionList = this.countryObject.regionACList;
      if (this.entityCountryId === '' || this.entityCountryId === undefined || this.entityCountryId === '0') {
        this.countryObject.regionId = this.countryObject.regionACList[0].id;
      }
    }, error => {
      this.handleError(error);
    });
  }

  /**
   * Only on enter press get all the countries on the search term
   * @param event : Key board event that is pressed
   * @param searchValue : Search string
   */
  passValueOnEnterEvent(event: KeyboardEvent, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText && searchValue.trim() !== '') {
      this.isLoading = true;
      this.isToHideList = true;
      this.subject.next(searchValue);
    }
    this.isToHideList = true;
    if (searchValue.trim() === '') {
      this.isDataPopulated = false;
      this.countryObject.countryName = '';
      // if no serach string entered or blanked then refresh form data
      this.isNoRecordMatched = false;
    }
  }
  /**
   * Get all countries based on seraching
   */
  getCountryBasedOnSearch() {
    this.dropdownList = this.subject.pipe(
      switchMap((searchText: string) => {
        return this.countryService.countriesGetAllCountryBasedOnSearch(searchText, this.selectedEntityId).pipe(map((result) => {
          this.isLoading = false;
          this.countryObject.countryACList = JSON.parse(JSON.stringify(result.countryACList));
          this.tempCountryList = this.countryObject.countryACList;
          // for showing countrynames in dropdown
          for (const countryName of this.countryObject.countryACList) {
            this.countryList.push(countryName.name);
          }
          // if no record found so error message
          this.isNoRecordMatched = this.isToHideList = this.countryList.length === 0;
          return this.countryObject.countryACList;

        }));
      })
    );
  }

  /**
   * Populate other data according to the selection of names
   * @param countryName : countryName
   */
  populateSelectedCountryDetails(countryName: string) {
    this.isToHideList = true;
    this.isDataPopulated = true;
    this.countryObject.countryName = this.tempCountryList.filter(x => x.name === countryName)[0].name;
  }

  /**
   * Method for saving/updating country details
   */
  saveCountry() {
    // remove extra spaces foreach field
    this.countryObject.countryName = this.countryObject.countryName.trim();
    this.countryObject.entityId = this.selectedEntityId;
    const checkCountryExist = this.tempCountryList.findIndex(x => x.name === this.countryObject.countryName.trim());
    this.countryObject.countryId = this.tempCountryList.length > 0 && checkCountryExist !== -1 ? this.tempCountryList.filter(x => x.name === this.countryObject.countryName.trim())[0].id : '';
    this.countryObject.regionName = this.tempRegionList.length > 0 ? this.tempRegionList.filter(x => x.id === this.countryObject.regionId)[0].name : '';
    this.countryObject.countryACList = [];
    if (this.entityCountryId === undefined || this.entityCountryId === '' || this.entityCountryId === '0') {
      this.countryService.countriesAddCountry(this.countryObject, this.selectedEntityId).subscribe((result: EntityCountryAC) => {

        this.bsModalRef.hide();
        this.bsModalRef.content.callback(result);
      }, error => {
          this.handleError(error);
      });
    } else {
      // NOTE : only on value change related implementaion pending
      // if form value changed then only update
      this.countryService.countriesUpdateCountry(this.countryObject, this.selectedEntityId)
        .subscribe(() => {
          this.bsModalRef.hide();
          this.bsModalRef.content.callback(this.countryObject);
        }, error => {
            this.handleError(error);
        });
    }
  }

  /**
   * Handle error scenario in case of add/ update
   * @param error : http error
   */
  handleError(error: HttpErrorResponse) {
    this.loaderService.close();
    if (error.status === 403) {
      // if entity close then show info of close status
      this.sharedService.showInfo(this.stringConstants.closedEntityRestrictionMessage);

    } else if (error.status === 405) {
      // if entity is deleted then show warning for the action
      this.sharedService.showWarning(this.stringConstants.deletedEntityRestrictionMessage);
    } else {

      // check if duplicate entry exception then show error message otherwise show something went wrong message
      const errorMessage = error.error !== null ? error.error : this.stringConstants.somethingWentWrong;
      this.sharedService.showError(errorMessage);
    }
  }

}
