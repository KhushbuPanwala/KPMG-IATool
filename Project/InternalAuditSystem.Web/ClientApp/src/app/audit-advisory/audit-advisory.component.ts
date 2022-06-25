import { Component, OnInit } from '@angular/core';
import { StringConstants } from '../shared/stringConstants';
import { SharedService } from '../core/shared.service';
import { LoaderService } from '../core/loader.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ObservationsManagementService, ObservationAC, ReportAC, ReportsService, ACMPresentationAC, AcmService } from '../swaggerapi/AngularFiles';
import { Pagination } from '../models/pagination';

@Component({
  selector: 'app-audit-advisory',
  templateUrl: './audit-advisory.component.html',
})
export class AuditAdvisoryComponent implements OnInit {
  auditAdvisoryTitle: string; // Variable for audit advisory page title
  selectAreaTitle: string; // Variable for select area title
  yearTitle: string; // Variable year title
  headingLabel: string; // Variable Heading title
  observationTabTitle: string; // Variable for observation title
  recommendationTitle: string; // Variable for recommendation
  managementResponseTitle: string; // Variable for management response
  ratingLabel: string; // Variable for rating label
  implicationTitle: string; // Variable for implication title
  statusTitle: string; // Variable for status title
  showingResults: string; // Variable for showing results
  processLabel: string; // Variable for process label
  auditorTitle: string; // Variable for auditor title
  personResponsibleLabel: string; // Variable for person responsible label
  reportTitle: string; // Variable for report title
  noOfObservationTitle: string; // Variable for no of observation
  versionLabel: string; // Variable for version title
  periodTitle: string; // VAriable for persion title
  stageTitle: string; // Variable for stage title
  acmTitle: string; // Variable for acm title
  searchText: string; // Variable for search text
  auditableEntityId: string;
  searchValue = null;
  selectedPageItem: number; // per page no. of items shows
  pageNumber: number = null;
  totalRecords: number;
  yearList: number[] | string[];
  // observation section
  observationList = [] as Array<ObservationAC>;
  isObservation: boolean;
  isAcm: boolean;
  isReportManagement: boolean;
  showingText: string;
  showNoDataText: string;
  pageItems = [];
  addToolTip: string; // Vairable for add tooltip
  selectedFromYear: number;
  selectedToYear: number;
  selectedYear: string;

  // report section
  reportList = [] as Array<ReportAC>;

  // acm section
  acmList = [] as Array<ACMPresentationAC>;
  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };

  // TODO: Added static code here, respective developer will change it in future
  // Status items for acm report
  areaList = [
    {
      item: 'Report Management',
    },
    {
      item: 'Observation Management'
    },
    {
      item: 'ACM Presentation',
    }
  ];
  selectedAreaList = this.areaList[0].item;

  // TODO: Added static code here, respective developer will change it in future
  // Year items for audit advisory
  yearItems = [
    {
      item: '2019 - 2020',
    },
    {
      item: '2018 - 2019'
    },
    {
      item: '2017 - 2018',
    }
  ];
  selectedYearItems = this.yearItems[0].item;

  // Creates an instance of documenter
  constructor(private stringConstants: StringConstants, private sharedService: SharedService, private loaderService: LoaderService,
              private router: Router, private route: ActivatedRoute, private observationService: ObservationsManagementService,
              private reportApiService: ReportsService , private acmServices: AcmService) {
    this.auditAdvisoryTitle = this.stringConstants.auditAdvisoryTitle;
    this.selectAreaTitle = this.stringConstants.selectAreaTitle;
    this.yearTitle = this.stringConstants.yearTitle;
    this.headingLabel = this.stringConstants.headingLabel;
    this.observationTabTitle = this.stringConstants.observationTabTitle;
    this.recommendationTitle = this.stringConstants.recommendationTitle;
    this.managementResponseTitle = this.stringConstants.managementResponseTitle;
    this.ratingLabel = this.stringConstants.ratingLabel;
    this.implicationTitle = this.stringConstants.implicationTitle;
    this.statusTitle = this.stringConstants.statusTitle;
    this.showingResults = this.stringConstants.showingResults;
    this.processLabel = this.stringConstants.processLabel;
    this.auditorTitle = this.stringConstants.auditorTitle;
    this.personResponsibleLabel = this.stringConstants.personResponsibleLabel;
    this.reportTitle = this.stringConstants.reportTitle;
    this.noOfObservationTitle = this.stringConstants.noOfObservationTitle;
    this.ratingLabel = this.stringConstants.ratingLabel;
    this.stageTitle = this.stringConstants.stageTitle;
    this.versionLabel = this.stringConstants.versionLabel;
    this.periodTitle = this.stringConstants.periodTitle;
    this.acmTitle = this.stringConstants.acmTitle;
    this.searchText = this.stringConstants.searchText;
    this.auditableEntityId = '';
    this.isObservation = false;
    this.isAcm = false;
    this.isReportManagement = false;
    this.showingText = this.stringConstants.showingText;
    this.pageItems = this.stringConstants.pageItems;
    this.selectedPageItem = this.pageItems[0].noOfItems;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.addToolTip = this.stringConstants.addToolTip;
    this.selectedYear = this.yearItems[0].item;
  }

  /** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {

    this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      this.auditableEntityId = entityId;
      this.getYearList();
      this.route.params.subscribe(params => {
        if (params.pageItems !== undefined) {
          this.selectedPageItem = Number(params.pageItems);
        }
        this.searchValue = (params.searchValue !== undefined && params.searchValue !== null) ? params.searchValue : '';
      });

    });
  }

  /**
   * Method for getting data of observation year wise
   * @param pageNumber : current page number
   * @param selectedPageItem :no. of items display on per page
   * @param searchValue :search value
   * @param entityId : auditable entityId
   * @param fromYear : selected from year
   * @param toYear : selected to year
   */
  getObservationList(pageNumber: number, selectedPageItem: number, searchValue: string, entityId: string, fromYear: number, toYear: number) {
    this.loaderService.open();
    this.observationService.observationsManagementGetAllObservations(pageNumber, selectedPageItem, searchValue, entityId, fromYear, toYear).subscribe((result: Pagination<ObservationAC>) => {
      this.observationList = result.items;
      this.pageNumber = result.pageIndex;
      this.totalRecords = result.totalRecords;
      this.searchValue = searchValue;
      this.showingResults = this.sharedService.setShowingResult(result.pageIndex, result.pageSize, result.totalRecords);
      this.loaderService.close();
    }, (error) => {
      this.loaderService.close();
      this.sharedService.showError(this.stringConstants.somethingWentWrong);
    });
  }

  /**
   * Method for getting report observations data year wise
   * @param pageNumber : current page number
   * @param selectedPageItem :no. of items display on per page
   * @param searchValue :search value
   * @param entityId : auditable entityId
   * @param fromYear : selected from year
   * @param toYear : selected to year
   */
  getReportManagementList(pageNumber: number, selectedPageItem: number, searchValue: string, entityId: string, fromYear: number, toYear: number) {
    this.loaderService.open();
    this.reportApiService.reportsGetReports(pageNumber, selectedPageItem, searchValue, entityId, fromYear, toYear).subscribe((result: Pagination<ReportAC>) => {
      this.reportList = result.items;
      this.pageNumber = result.pageIndex;
      this.totalRecords = result.totalRecords;
      this.showingResults = this.sharedService.setShowingResult(this.pageNumber, selectedPageItem, this.totalRecords);
      this.loaderService.close();
    }, (error) => {
      this.loaderService.close();
      this.sharedService.showError(this.stringConstants.somethingWentWrong);
    });
  }

  /**
   * Method for getting acm list year wise
   * @param pageNumber : current page number
   * @param selectedPageItem :no. of items display on per page
   * @param searchValue :search value
   * @param entityId : auditable entityId
   * @param fromYear : selected from year
   * @param toYear : selected to year
   */
  getAcmList(pageNumber: number, selectedPageItem: number, searchValue: string, fromYear: number, toYear: number) {
    this.loaderService.open();
    this.acmServices.acmGetACMData(pageNumber, selectedPageItem, searchValue, this.auditableEntityId, fromYear, toYear).subscribe((result: Pagination<ACMPresentationAC>) => {
      this.acmList = result.items;
      this.pageNumber = result.pageIndex;
      this.totalRecords = result.totalRecords;
      this.searchValue = searchValue;
      this.showingResults = this.sharedService.setShowingResult(this.pageNumber, selectedPageItem, this.totalRecords);
      this.loaderService.close();
    }, (error) => {
      this.loaderService.close();
      this.sharedService.showError(this.stringConstants.somethingWentWrong);
    });
  }

  /**
   * Open observation list page
   * @param reportId: current report id
   */
  openObservationList(reportId: string) {
    this.router.navigate(['report/observation-list', { id: reportId, type: this.stringConstants.editOperationText }]);
  }

  /**
   * Method for getting current selected area
   * @param selectedArea : selected area
   */
  onAreaChange(selectedArea: string) {
    if (selectedArea === this.stringConstants.observationManagementTitle) {
      this.isObservation = true;
      this.isReportManagement = false;
      this.isAcm = false;
      this.observationList = [];
    } else if (selectedArea === this.stringConstants.reportManagemenTitle) {
      this.isReportManagement = true;
      this.isObservation = false;
      this.isAcm = false;
      this.reportList = [];
    } else {
      this.isAcm = true;
      this.isReportManagement = false;
      this.isObservation = false;
    }
  }

  /**
   * Method for getting current selected year
   * @param selectedYear : Selected year
   */
  onYearChange(selectedYear: string) {
    this.selectedYear = selectedYear;
  }

  /**
   * Method for showing data based on selection of area and year
   * @param selectedAreaList : List of area
   * @param selectedYear :List of year
   */
  showData(selectedAreaList, selectedYear: string) {
    this.onAreaChange(selectedAreaList);
    const fromYear = selectedYear.split('-')[0];
    const toYear = selectedYear.split('-')[1];
    this.selectedFromYear = parseInt(fromYear, 10);
    this.selectedToYear = parseInt(toYear, 10);
    this.searchValue = '';
    if (this.isObservation) {
      this.getObservationList(this.pageNumber, this.selectedPageItem, this.searchValue, this.auditableEntityId, this.selectedFromYear, this.selectedToYear);
    } else if (this.isReportManagement) {
      this.getReportManagementList(this.pageNumber, this.selectedPageItem, this.searchValue, this.auditableEntityId, this.selectedFromYear, this.selectedToYear);
    } else {
      this.getAcmList(this.pageNumber, this.selectedPageItem, this.searchValue, this.selectedFromYear, this.selectedToYear);
    }
  }

  /**
   * On change current page
   * @param pageNumber:page no. which is user selected
   * @param isAcm:true if acm is selected
   * @param isReportManagement:true if report management is selected
   * @param isObservation:true if observation is selected
   */
  onPageChange(pageNumber, isAcm, isReportManagement, isObservation) {
    if (isObservation) {
      this.getObservationList(pageNumber, this.selectedPageItem, this.searchValue, this.auditableEntityId, this.selectedFromYear, this.selectedToYear);
    } else if (isReportManagement) {
      this.getReportManagementList(pageNumber, this.selectedPageItem, this.searchValue, this.auditableEntityId, this.selectedFromYear, this.selectedToYear);
    } else {
      this.getAcmList(this.pageNumber, this.selectedPageItem, this.searchValue, this.selectedFromYear, this.selectedToYear);
    }
  }

  /**
   * On per page items change
   * @param isAcm : true if acm is selected
   * @param isReportManagement : true if report management is selected
   * @param isObservation : true if observation is selected
   */
  onPageItemChange(isAcm, isReportManagement, isObservation) {
    if (isObservation) {
      this.getObservationList(null, this.selectedPageItem, this.searchValue, this.auditableEntityId, this.selectedFromYear, this.selectedToYear);
    } else if (isReportManagement) {
      this.getReportManagementList(null, this.selectedPageItem, this.searchValue, this.auditableEntityId, this.selectedFromYear, this.selectedToYear);
    } else {
      this.getAcmList(this.pageNumber, this.selectedPageItem, this.searchValue, this.selectedFromYear, this.selectedToYear);
    }
  }

  /**
   * Method for searching acm,observtion,report management data from textbox
   * @param event : key press event
   * @param pageNumber : current page number
   * @param selectedPageItem :no. of items display on per page
   * @param searchValue :search value
   * @param isAcm : true if acm is selected
   * @param isReportManagement : true if report management is selected
   * @param isObservation : true if observation management is selected
   */
  searchData(event: KeyboardEvent, pageNumber: number, selectedPageItem: number, searchValue: string, isAcm: boolean, isReportManagement: boolean, isObservation: boolean) {
    this.isAcm = isAcm;
    this.isObservation = isObservation;
    this.isReportManagement = isReportManagement;

    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      if (this.isObservation) {
        this.getObservationList(pageNumber, selectedPageItem, searchValue, this.auditableEntityId, this.selectedFromYear, this.selectedToYear);
      } else if (this.isReportManagement) {
        this.getReportManagementList(pageNumber, selectedPageItem, searchValue, this.auditableEntityId, this.selectedFromYear, this.selectedToYear);
      } else {
        this.getAcmList(this.pageNumber, this.selectedPageItem, this.searchValue,  this.selectedFromYear, this.selectedToYear);
      }
    }
  }

  /**
   * Method for getting year list
   */
  getYearList() {
    const year = new Date().getFullYear();
    const startRange = [];
    startRange.push((year - 1) + '-' + year);

    for (let i = 1; i < 7; i++) {
      startRange.push((year - i) + '-' + (year + i));
    }
    this.yearList = startRange;
  }
}
