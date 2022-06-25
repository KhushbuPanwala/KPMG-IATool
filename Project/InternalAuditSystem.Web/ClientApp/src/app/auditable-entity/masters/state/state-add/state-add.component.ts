import { Component, OnInit } from '@angular/core';
import { StringConstants } from '../../../../shared/stringConstants';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { SharedService } from '../../../../core/shared.service';
import { EntityStateAC, StatesService, ProvinceStateAC, EntityCountryAC } from '../../../../swaggerapi/AngularFiles';
import { LoaderService } from '../../../../core/loader.service';
import { Subject, Observable } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-state-add',
  templateUrl: './state-add.component.html'
})
export class StateAddComponent implements OnInit {

  regionText: string; // Variable for region text
  countryText: string; // Variable for country text
  stateText: string; // Variable for state text
  saveButtonText: string; // Variable for save button text
  selected: string; // Variable for selected state text
  choose: string; // Variable for choose placeholder
  selectedEntityId: string;
  entityStateId: string;
  stateObject = {} as EntityStateAC;
  stateList: string[] = [];
  tempStateList: ProvinceStateAC[] = [];
  tempCountryList: EntityCountryAC[] = [];
  countryId: string;
  public regionName: string;
  isLoading = false;
  isToHideList = true;
  isDataPopulated = false;
  isNoRecordMatched: boolean;
  displayNameList = [] as Array<string>;
  dropdownList: Observable<ProvinceStateAC[]>;
  subject = new Subject<string>();
  requiredMessage: string;
  noRecordFound: string;
  maxLengthExceedMessage: string;
  invalidMessage: string;

  // Creates an instance of documenter
  constructor(private stringConstants: StringConstants,
              public bsModalRef: BsModalRef, private sharedService: SharedService, private stateService: StatesService,
              private loaderService: LoaderService) {
    this.regionText = this.stringConstants.regionText;
    this.countryText = this.stringConstants.countryText;
    this.stateText = this.stringConstants.stateText;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.choose = this.stringConstants.choose;
    this.regionName = '';
    this.countryId = '';
    this.requiredMessage = this.stringConstants.stateRequiredMessage;
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
      this.entityStateId = this.stateObject.id;
      this.getStateBasedOnSearch();
      this.getStateById(this.entityStateId, this.selectedEntityId);
    });
  }

  /**
   * Method to get regionName on selection of country
   * @param countryId : Id of country
   */
  onCountryChanged(countryId: string) {
    this.stateObject.regionName = this.stateObject.countryACList.filter(x => x.countryId === countryId)[0]?.regionName;
    this.stateObject.entityCountryId = this.stateObject.countryACList.filter(x => x.countryId === countryId)[0]?.id;
  }

  /**
   * Method for getting data based on id
   * @param entityStateId : Id of entityState
   * @param entityId : Id of selected entity
   */
  getStateById(entityStateId: string, entityId: string) {
    this.stateService.statesGetStateById(entityStateId, entityId).subscribe(result => {
      this.stateObject.countryACList = JSON.parse(JSON.stringify(result.countryACList));
      this.tempCountryList = this.stateObject.countryACList;
      if (this.entityStateId === '' || this.entityStateId === undefined || this.entityStateId === '0') {
        this.stateObject.entityCountryId = this.stateObject.countryACList.length > 0 ? this.stateObject.countryACList[0].id : '';
        this.countryId = this.stateObject.countryACList.length > 0 ? this.stateObject.countryACList[0].countryId : '';
        this.stateObject.regionName = this.stateObject.countryACList.length > 0 ? this.stateObject.countryACList[0].regionName : '';
      } else {
        this.stateObject.entityCountryId = this.stateObject.countryACList.length > 0 ? this.stateObject.countryACList.filter(x => x.id === result.entityCountryId)[0].id : '';
        this.countryId = this.stateObject.countryACList.length > 0 ? this.stateObject.countryACList.filter(x => x.id === result.entityCountryId)[0].countryId : '';
        this.stateObject.regionName = this.stateObject.countryACList.length > 0 ? this.stateObject.countryACList.filter(x => x.id === result.entityCountryId)[0].regionName : '';
      }
    });
  }

  /**
   * Only on enter press get all the states based on the search term
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
      // if no serach string entered or blanked then refresh form data
      this.stateObject.stateName = '';
      this.isNoRecordMatched = false;
    }
  }

  /**
   * Get all states based on seraching
   */
  getStateBasedOnSearch() {
    this.dropdownList = this.subject.pipe(
      switchMap((searchText: string) => {
        return this.stateService.statesGetAllStatesBasedOnSearch(searchText, this.selectedEntityId).pipe(map((result) => {
          this.isLoading = false;
          this.stateObject.stateACList = JSON.parse(JSON.stringify(result.stateACList));
          this.tempStateList = this.stateObject.stateACList;
          // for showing countrynames in dropdown
          for (const stateName of this.stateObject.stateACList) {
            this.stateList.push(stateName.name);
          }
          // if no record found so error message
          this.isNoRecordMatched = this.isToHideList = this.stateList.length === 0;
          return this.stateObject.stateACList;

        }));
      })
    );
  }

  /**
   * Populate other data according to the selection of names
   * @param stateName : stateName
   */
  populateSelectedStateDetails(stateName: string) {
    this.isToHideList = true;
    this.isDataPopulated = true;
    this.stateObject.stateName = this.tempStateList.filter(x => x.name === stateName)[0].name;
  }

  /**
   * Method for saving state
   */
  saveState() {
    // remove extra spaces foreach field
    this.stateObject.stateName = this.stateObject.stateName !== undefined ? this.stateObject.stateName.trim() : '';
    this.stateObject.entityId = this.selectedEntityId;
    const checkStateExist = this.tempStateList.findIndex(x => x.name === this.stateObject.stateName.trim());
    this.stateObject.stateId = checkStateExist !== -1 && this.tempStateList.length > 0 ? this.tempStateList.filter(x => x.name === this.stateObject.stateName)[0].id : '';
    this.stateObject.countryName = this.tempCountryList.length > 0 ? this.tempCountryList.filter(x => x.countryId === this.countryId)[0].countryName : '';
    this.stateObject.countryACList = [];
    this.stateObject.stateACList = [];

    if (this.entityStateId === undefined || this.entityStateId === '' || this.entityStateId === '0') {
      this.stateService.statesAddState(this.stateObject, this.selectedEntityId).subscribe((result: EntityStateAC) => {
        this.bsModalRef.hide();
        this.bsModalRef.content.callback(result);
      }, error => {
          this.handleError(error);
      });
    } else {
      // NOTE : only on value change related implementaion pending
      // if form value changed then only update
      this.stateService.statesUpdateState(this.stateObject, this.selectedEntityId)
        .subscribe(() => {
          this.bsModalRef.hide();
          this.bsModalRef.content.callback(this.stateObject);
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
