import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../../../core/shared.service';
import { AgGridEvent } from 'ag-grid-community';
import { ProcessAC, ReportObservationsService, ReportDetailAC, AuditPlanAC, PlanProcessMappingAC, ReportObservationAC } from '../../../swaggerapi/AngularFiles';
import { LoaderService } from '../../../core/loader.service';
import { AgGridCheckboxComponent } from '../ag-grid-component/ag-grid-checkbox.component';
import { AgGridViewButtonComponent } from '../ag-grid-component/ag-grid-view-button.component';
import { AgGridEditButtonComponent } from '../ag-grid-component/ag-grid-edit-button.component';
import { AgGridDeleteButtonComponent } from '../ag-grid-component/ag-grid-delete-button.component';
import { ReportSharedService } from '../report-shared.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-observation-list',
  templateUrl: './observation-list.component.html',
  styleUrls: ['./observation-list.component.scss']
})
export class ObservationListComponent implements OnInit, OnDestroy {
  backToolTip: string; // Vairable for back button tooltip
  addToolTip: string; // Variable for add tooltip
  saveNextButtonText: string; // Varible for save button next
  showNoDataText: string;
  observationListLabel: string; // Variable for report title
  auditPlanLabel: string;
  processLabel: string;
  subProcessLabel: string;
  reportId;
  noOfObservation;
  selectedEntityId;
  auditPlanList = [] as Array<AuditPlanAC>;
  processList = [] as Array<ProcessAC>;
  subProcessList = [] as Array<PlanProcessMappingAC>;
  selectedPlan;
  selectedProcess;
  selectedSubProcess;
  event: AgGridEvent;
  observationsList = [] as Array<ReportObservationAC>;
  searchData = [] as Array<ReportObservationAC>;
  searchText: string; // Variable for search text
  excelToolTip: string; // Variable for excel tooltip
  uploadText: string;

  // drag and drop list property
  public gridApi;
  public gridColumnApi;
  public rowData = [] as Array<ReportObservationAC>;
  process = {} as ProcessAC;
  subProcess = {} as ProcessAC;
  selectedObservationIds = [] as Array<string>;
  reportObservations = {} as ReportDetailAC;
  selectedRows;
  isDisableSave = true;
  searchValue = '';
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  operationType: string;
  callFrom: string;
  dropdownDefaultValue: string;
  isCallAfterDelete = false;  // used to refresh grid after delete
  version: string;
  public defaultColDef;
  columnDefs = [
    {
      headerName: '',
      field: 'sortOrder',
      minWidth: 50,
      maxWidth: 70,
      rowDrag: true,
    },
    {
      headerName: '',
      field: 'isSelected',
      minWidth: 50,
      maxWidth: 70,
      cellRendererFramework: AgGridCheckboxComponent
    },
    {
      headerName: 'Heading',
      field: 'heading',
      minWidth: 150,
      tooltipField: 'heading'
    },
    {
      headerName: 'Audit Plan',
      field: 'auditPlanName',
      minWidth: 150,
      tooltipField: 'auditPlanName'
    },
    {
      headerName: 'Process',
      field: 'processName',
      minWidth: 150,
      tooltipField: 'processName'
    },
    {
      headerName: 'Sub Process',
      field: 'subProcessName',
      minWidth: 150,
      tooltipField: 'subProcessName'
    },
    {
      headerName: '',
      field: 'id',
      minWidth: 50,
      maxWidth: 70,
      cellRendererFramework: AgGridViewButtonComponent,
      cellClass: 'no-wrap',
      tooltipField: 'edit'
    },
    {
      headerName: '',
      field: 'id',
      minWidth: 50,
      maxWidth: 70,
      cellRendererFramework: AgGridEditButtonComponent,
      cellClass: 'no-wrap',
      tooltipField: 'view'
    },
    {
      headerName: '',
      minWidth: 50,
      maxWidth: 70,
      field: 'id',
      cellRendererFramework: AgGridDeleteButtonComponent,
      cellClass: 'no-wrap',
      tooltipField: 'delete'
    },

  ];


  gridOptions = {
    animateRows: true,
    enableSorting: true,
    enableCellChangeFlash: true,

    enableBrowserTooltips: true
  };

  // Creates an instance of documenter.
  constructor(
    private stringConstants: StringConstants,
    private apiService: ReportObservationsService,
    private router: Router,
    private route: ActivatedRoute,
    private sharedService: SharedService,
    private reportService: ReportSharedService,
    private loaderService: LoaderService
  ) {
    this.observationListLabel = this.stringConstants.observationListLabel;
    this.auditPlanLabel = this.stringConstants.auditPlanLabel;
    this.processLabel = this.stringConstants.processLabel;
    this.subProcessLabel = this.stringConstants.subProcessLabel;
    this.backToolTip = this.stringConstants.backToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.saveNextButtonText = this.stringConstants.saveNextButtonText;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.uploadText = this.stringConstants.uploadText;
    this.dropdownDefaultValue = this.stringConstants.dropdownDefaultValue;
    this.version = this.stringConstants.versionTitle;

    this.defaultColDef = { resizable: true };
  }
  onFirstDataRendered(params) {
    params.api.sizeColumnsToFit();
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
  *  Initialization of properties.
  */
  ngOnInit() {
    this.loaderService.close();
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;

        this.route.params.subscribe(params => {
          this.reportId = params.id;
          this.operationType = params.type;
        });
        if (this.operationType === this.stringConstants.addOperationText) {
          this.columnDefs = [
            {
              headerName: '',
              field: 'sortOrder',
              minWidth: 50,
              maxWidth: 70,
              rowDrag: true,
            },
            {
              headerName: '',
              field: 'isSelected',
              minWidth: 50,
              maxWidth: 70,
              cellRendererFramework: AgGridCheckboxComponent
            },
            {
              headerName: 'Heading',
              field: 'heading',
              minWidth: 150,
              tooltipField: 'heading'
            },
            {
              headerName: 'Audit Plan',
              field: 'auditPlanName',
              minWidth: 150,
              tooltipField: 'auditPlanName'
            },
            {
              headerName: 'Process',
              field: 'processName',
              minWidth: 150,
              tooltipField: 'processName'
            },
            {
              headerName: 'Sub Process',
              field: 'subProcessName',
              minWidth: 150,
              tooltipField: 'subProcessName'
            },
            {
              headerName: '',
              field: 'id',
              minWidth: 50,
              maxWidth: 70,
              cellRendererFramework: AgGridViewButtonComponent,
              cellClass: 'no-wrap',
              tooltipField: 'id'
            },
            {
              headerName: '',
              field: 'id',
              minWidth: 50,
              maxWidth: 70,
              cellRendererFramework: AgGridEditButtonComponent,
              cellClass: 'no-wrap',
              tooltipField: 'id'
            },
            {
              headerName: '',
              minWidth: 50,
              maxWidth: 70,
              field: 'id',
              cellRendererFramework: AgGridDeleteButtonComponent,
              cellClass: 'no-wrap',
              tooltipField: 'id'
            },
          ];
        }
        this.getPlanProcess();
      }
    });
    // After delete reload observation data
    this.reportService.selectedOperationDeletedSubject.subscribe(() => {
      if (this.reportService.isObservationDelete) {
        this.isCallAfterDelete = true;
        this.getPlanProcess();
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
   * Get Plan Process detail
   */
  getPlanProcess() {
    this.apiService.reportObservationsGetPlanProcesessInitData(this.selectedEntityId, this.reportId).subscribe(result => {
      this.auditPlanList = JSON.parse(JSON.stringify(result));
      if (this.operationType === this.stringConstants.editOperationText || this.operationType === this.stringConstants.viewOperationText) {
        this.onGridReady(this.event, true);
      }
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * On process change
   */
  onChangeAuditPlan() {
    const planDetail = this.auditPlanList.find(a => a.id === this.selectedPlan);
    this.processList = JSON.parse(JSON.stringify(planDetail.parentProcessList));
    this.selectedProcess = null;
    this.subProcessList = [] as Array<PlanProcessMappingAC>;
    this.selectedSubProcess = null;
  }
  /**
   * On process change
   */
  onChangeProcess() {
    const planIndex = this.auditPlanList.findIndex(a => a.id === this.selectedPlan);
    this.subProcessList = this.auditPlanList[planIndex].planProcessList.filter(a => a.parentProcessId === this.selectedProcess);
    this.selectedSubProcess = null;
  }

  /**
   * Get observation data and check validation
   */
  getObservations() {
    if (this.selectedPlan === null || this.selectedProcess === null || this.selectedSubProcess === null) {
      this.sharedService.showError(this.stringConstants.invalidSelection);
    } else {
      this.onGridReady(this.event, true);
    }
  }
  /**
   * Get observation details
   * @param params: aggrid event
   * @param callFrom: check for call from which methods
   */
  onGridReady(params: AgGridEvent, callFrom: boolean) {
    if (!callFrom) {
      this.gridApi = params.api;
      this.gridColumnApi = params.columnApi;
    }
    this.apiService.reportObservationsGetAllObservations(this.selectedPlan, this.selectedSubProcess, this.selectedEntityId, this.reportId).subscribe(result => {
      if (result.reportObservationList.length === 0 && !callFrom && !this.isCallAfterDelete) {
        this.sharedService.showError(this.stringConstants.noRecordFoundMessage);
      } else {
        for (const row of result.reportObservationList) {
          // set audit plan name
          const planDetail = this.auditPlanList.find(a => a.id === row.auditPlanId);
          row.auditPlanName = planDetail.title;
          // set sub process name
          this.subProcessList = planDetail.planProcessList.filter(a => a.processId === row.processId);
          row.subProcessName = this.subProcessList.find(a => a.processId === row.processId).processName;
          // set process name
          const processId = this.subProcessList.find(a => a.processId === row.processId).parentProcessId;
          this.processList = JSON.parse(JSON.stringify(planDetail.parentProcessList));
          row.processName = this.processList.find(a => a.id === processId).name;
        }
        if (this.observationsList.length === 0) {
          for (const row of result.reportObservationList) {
            this.observationsList.push(row);
          }
        } else {
          // compare two list and remove repeated data
          const newObservationList = result.reportObservationList;
          const agGridObservationList = this.observationsList;
          const notMatchedObservations = newObservationList.filter(function(o1) {
            // filter out (!) items in result2
            return !agGridObservationList.some(function(o2) {
              return o1.observationId === o2.observationId; // assumes unique id
            });
          }).map(function(o) {
            return o;
          });
          if (notMatchedObservations.length === 0 && callFrom && !this.isCallAfterDelete) {
            this.sharedService.showError(this.stringConstants.recordExistMsg);
          } else {
            for (const row of notMatchedObservations) {
              this.observationsList.push(row);
            }
          }
        }
        if (this.isCallAfterDelete) {
          this.rowData = JSON.parse(JSON.stringify(result.reportObservationList));

        } else {
          this.rowData = JSON.parse(JSON.stringify(this.observationsList));
        }
      }
      this.selectedPlan = null;
      this.selectedSubProcess = null;
      this.selectedProcess = null;
      this.processList = [] as Array<ProcessAC>;
      this.subProcessList = [] as Array<PlanProcessMappingAC>;

      let sortOrder = 1;
      for (const row of this.rowData) {
        row.sortOrder = sortOrder;
        sortOrder = sortOrder + 1;
      }
      this.observationsList = this.rowData;
      this.reportService.observationList = [] as Array<ReportObservationAC>;
      for (const row of this.observationsList) {
        if (row.isSelected) {
          this.reportService.observationList.push(row);
        }
      }
      this.isCallAfterDelete = false;
      this.reportService.updateDeletedObservationList(this.isCallAfterDelete);
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * Search observation
   * @param event: Keyboard Event
   * @param searchValue: search value
   */
  searchObservation(event: KeyboardEvent, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.searchData = [];
      if (searchValue !== '') {

        for (const row of this.observationsList) {
          if (row.heading.toLowerCase().includes(searchValue.toLowerCase())) {
            this.searchData.push(row);
          }
        }
        this.rowData = this.searchData;
      } else {
        this.rowData = this.observationsList;
      }
    }
  }
  /**
   * Save selected observation Detaila
   */
  saveReportObservations() {
    this.reportObservations.processId = this.selectedProcess;
    this.reportObservations.subProcessId = this.selectedSubProcess;
    this.reportObservations.entityId = this.selectedEntityId;
    this.reportObservations.reportId = this.reportId;
    this.reportObservations.reportObservationList = [] as Array<ReportObservationAC>;
    this.reportService.observationListSubject.subscribe((observationsList) => {
      this.observationsList = this.reportService.observationList;
    });
    for (const row of this.observationsList) {
      if (row.isSelected) {
        this.reportObservations.reportObservationList.push(row);
      }
    }
    if (this.reportObservations.reportObservationList.length === 0) {
      this.sharedService.showError(this.stringConstants.selectObservationMsg);
      this.apiService.reportObservationsAddObservations(this.reportObservations, this.selectedEntityId).subscribe(result => {
        this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
        this.router.navigate(['report/list', { id: this.reportId }]);
      },
        (error) => {
          this.sharedService.handleError(error);
        });

    } else {
      this.apiService.reportObservationsAddObservations(this.reportObservations, this.selectedEntityId).subscribe(result => {
        if (this.operationType === this.stringConstants.addOperationText) {
          this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
        } else {
          this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
        }
        this.router.navigate(['report/observation-add', { id: this.reportId, type: this.operationType }]);
      },
        (error) => {
          this.sharedService.handleError(error);
        });
    }
  }

  /**
   * on back button route to list page
   */
  onBackClick() {
    this.router.navigate(['report/distribution-add', { id: this.reportId, type: this.stringConstants.editOperationText }]);
  }
  /**
   * Add observation in given index
   * @param array: list of observations
   * @param index: index to add observation
   * @param elements: new added observation
   */
  insertAt(array: Array<ReportObservationAC>, index: number, elements: ReportObservationAC) {
    array.splice(index, 0, elements);
  }

  /**
   * Drag event of ag grid
   * @param e: any event of drag element
   */
  onRowDragEnd(e) {
    const selectedIndex = this.observationsList.findIndex(a => a.id === e.node.data.id);
    const selectedObservation = this.observationsList.find(a => a.id === e.node.data.id);
    this.observationsList.splice(selectedIndex, 1);
    this.insertAt(this.observationsList, e.overIndex, selectedObservation);

    // set order of observation
    for (let i = 0; i < this.observationsList.length; i++) {
      this.observationsList[i].sortOrder = i + 1;
    }
  }
}
