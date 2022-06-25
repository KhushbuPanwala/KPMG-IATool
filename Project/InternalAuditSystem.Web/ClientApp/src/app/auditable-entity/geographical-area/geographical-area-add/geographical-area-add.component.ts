import { Component, OnInit } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../../../core/shared.service';
import { LoaderService } from '../../../core/loader.service';
import { CountryAC } from '../../../swaggerapi/AngularFiles/model/countryAC';
import { RegionAC } from '../../../swaggerapi/AngularFiles/model/regionAC';
import { LocationAC, PrimaryGeographicalAreasService } from '../../../swaggerapi/AngularFiles';
import { ProvinceStateAC } from '../../../swaggerapi/AngularFiles/model/provinceStateAC';
import { PrimaryGeographicalAreaAC } from '../../../swaggerapi/AngularFiles/model/primaryGeographicalAreaAC';

@Component({
  selector: 'app-geographical-area-add',
  templateUrl: './geographical-area-add.component.html',
})
export class GeographicalAreaAddComponent implements OnInit {
  title: string;
  primaryAreasTitle: string; // Variable geographical area title
  saveButtonText: string; // Variable for save button
  regionText: string; // Variable for region text
  countryText: string; // Variable for country text
  stateText: string; // Variable for state
  locationLabel: string; // Variable for location label
  entityId: string;
  dropdownDefaultValue: string;
  primaryGeographicalAreaId: string;
  requiredMessage: string;

  countryList: CountryAC[];
  regionList: RegionAC[] = [];
  locationList: LocationAC[] = [];
  provinceStateList: ProvinceStateAC[] = [];

  primaryGeographicalAreaDetail: PrimaryGeographicalAreaAC;

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    public bsModalRef: BsModalRef,
    private route: ActivatedRoute,
    public router: Router,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private primaryGeographicalAreasService: PrimaryGeographicalAreasService) {
    this.primaryAreasTitle = this.stringConstants.primaryAreasTitle;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.regionText = this.stringConstants.regionText;
    this.countryText = this.stringConstants.countryText;
    this.stateText = this.stringConstants.stateText;
    this.locationLabel = this.stringConstants.locationLabel;
    this.dropdownDefaultValue = this.stringConstants.dropdownDefaultValue;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.primaryGeographicalAreaDetail = {} as PrimaryGeographicalAreaAC;
    this.countryList = [];
    this.provinceStateList = [];
  }

  /** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    this.getPrimaryGeographicalAreaDetails();
  }

  /**
   * Method to get PrimaryGeographicalArea Details
   */
  getPrimaryGeographicalAreaDetails() {
    this.loaderService.open();
    this.primaryGeographicalAreasService.primaryGeographicalAreasGetPrimaryGeographicalAreaDetails(this.entityId, this.primaryGeographicalAreaId).subscribe((result: PrimaryGeographicalAreaAC) => {
      this.regionList = result.regionACList;
      this.locationList = result.locationACList;

      if (this.primaryGeographicalAreaId !== '') {
        this.onRegionSelection(result.regionId);
        this.onCountrySelection(result.entityCountryId);
        this.primaryGeographicalAreaDetail = result;
      }

      this.loaderService.close();
    },
      (error) => {
        this.loaderService.close();
        this.sharedService.showError(error.error);
      });
  }

  /**
   * Method On region selection
   * @param id: region id
   */
  onRegionSelection(id: string) {
    this.countryList = [];
    this.provinceStateList = [];
    this.loaderService.open();

    this.primaryGeographicalAreaDetail.entityCountryId = '';
    this.primaryGeographicalAreaDetail.entityStateId = '';
    this.primaryGeographicalAreasService.primaryGeographicalAreasGetCountryListByRegion(id, this.entityId).subscribe((result: CountryAC[]) => {
      this.countryList = result;
      this.loaderService.close();
    },
      (error) => {
        this.loaderService.close();
        this.sharedService.showError(error.error);
      });
  }

  /**
   * Method On country selection
   * @param id: country id
   */
  onCountrySelection(id: string) {
    this.loaderService.open();
    this.provinceStateList = [];
    this.primaryGeographicalAreaDetail.entityStateId = '';
    this.primaryGeographicalAreasService.primaryGeographicalAreasGetStateByCountryId(id, this.entityId).subscribe((result: ProvinceStateAC[]) => {
      this.provinceStateList = result;
      this.loaderService.close();
    },
      (error) => {
        this.loaderService.close();
        this.sharedService.showError(error.error);
      });
  }

  /**
   * Add or update PrimaryGeographicalArea
   */
  savePrimaryGeographicalArea() {
    this.loaderService.open();

    if (this.primaryGeographicalAreaId === '') {
      this.primaryGeographicalAreaDetail.entityId = this.entityId;

      this.primaryGeographicalAreasService.primaryGeographicalAreasAddPrimaryGeographicalArea(this.primaryGeographicalAreaDetail).subscribe((result: PrimaryGeographicalAreaAC) => {
        this.bsModalRef.hide();
        result.regionString = this.regionList.filter(x => x.id === this.primaryGeographicalAreaDetail.regionId)[0].name;
        result.countryString = this.countryList.filter(x => x.id === this.primaryGeographicalAreaDetail.entityCountryId)[0].name;
        result.stateString = this.provinceStateList.filter(x => x.id === this.primaryGeographicalAreaDetail.entityStateId)[0].name;
        result.locationString = this.locationList.filter(x => x.id === this.primaryGeographicalAreaDetail.locationId)[0].name;
        this.loaderService.close();

        this.bsModalRef.content.callback(result);

      },
        (error) => {
          this.loaderService.close();
          this.sharedService.showError(error.error);
        });
    } else {
      this.primaryGeographicalAreasService.primaryGeographicalAreasUpdatePrimaryGeographicalArea(this.primaryGeographicalAreaDetail).subscribe(() => {
        this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
        this.loaderService.close();
        this.bsModalRef.hide();
        this.primaryGeographicalAreaDetail.regionString = this.regionList.filter(x => x.id === this.primaryGeographicalAreaDetail.regionId)[0].name;
        this.primaryGeographicalAreaDetail.countryString = this.countryList.filter(x => x.id === this.primaryGeographicalAreaDetail.entityCountryId)[0].name;
        this.primaryGeographicalAreaDetail.stateString = this.provinceStateList.filter(x => x.id === this.primaryGeographicalAreaDetail.entityStateId)[0].name;
        this.primaryGeographicalAreaDetail.locationString = this.locationList.filter(x => x.id === this.primaryGeographicalAreaDetail.locationId)[0].name;
        this.bsModalRef.content.callback(this.primaryGeographicalAreaDetail);
      },
        (error) => {
          this.loaderService.close();
          this.sharedService.showError(error.error);
        });
    }

  }
}
