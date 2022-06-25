import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { StringConstants } from '../shared/stringConstants';
import { AuditPlanAC, AuditPlansService, AuditPlanSectionType, AuditPlanStatus, PlanProcessStatus } from '../swaggerapi/AngularFiles';
import { LoaderService } from '../core/loader.service';
import { Router } from '@angular/router';
import { SharedService } from '../core/shared.service';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BehaviorSubject } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class AuditPlanSharedService {
  isStatusChangedToClose: boolean;

  planStatus = [
    { value: AuditPlanStatus.NUMBER_0, label: 'Active' },
    { value: AuditPlanStatus.NUMBER_1, label: 'Update' },
    { value: AuditPlanStatus.NUMBER_2, label: 'Closed' }
  ];

  planProcessStatus = [
    { value: PlanProcessStatus.NUMBER_0, label: 'Inprogress' },
    { value: PlanProcessStatus.NUMBER_1, label: 'Closed' }
  ];

  constructor(
    public toastr: ToastrService,
    public stringConstants: StringConstants,
    private loaderService: LoaderService,
    private auditPlanService: AuditPlansService,
    private router: Router,
    private sharedService: SharedService) {
  }

  /**
   * Save and Redirect to new section
   * @param auditPlanDetails : Audit plan details
   * @param selectedEntityId : selected entity id
   * @param selectionType : current section type
   * @param isFromAddPage : is from add page or edit page
   */
  saveAndRediectToNextSection(auditPlanDetails: AuditPlanAC, selectedEntityId: string, selectionType: AuditPlanSectionType, isFromAddPage: string) {

    this.loaderService.open();
    let navigationPath: string;
    auditPlanDetails.sectionType = selectionType;
    const sucessMessage = isFromAddPage === this.stringConstants.trueString ? this.stringConstants.recordAddedMsg : this.stringConstants.recordUpdatedMsg;
    switch (selectionType) {

      // general tab
      case AuditPlanSectionType.NUMBER_0:
        auditPlanDetails.title = auditPlanDetails.title.trim();
        if (auditPlanDetails.selectedTypeId === undefined) {
          auditPlanDetails.selectedTypeId = null;
        }
        if (auditPlanDetails.selectCategoryId === undefined) {
          auditPlanDetails.selectCategoryId = null;
        }
        navigationPath = this.stringConstants.auditPlanOverviewPath;
        break;

      // overview tab
      case AuditPlanSectionType.NUMBER_1:
        auditPlanDetails.overviewBackground = auditPlanDetails.overviewBackground.trim();
        if (auditPlanDetails.totalBudgetedHours === null) {
          auditPlanDetails.totalBudgetedHours = 0;
        }
        navigationPath = this.stringConstants.auditPlanPlanProcessPath;
        break;
    }

    // make server call for saving
    if (auditPlanDetails.id === undefined) {
      this.auditPlanService.auditPlansAddAuditPlan(auditPlanDetails, selectedEntityId).subscribe((result: string) => {
        // redirect to next section
        this.redirectToNextSection(navigationPath, result, sucessMessage, isFromAddPage);
      }, error => {
        this.handleError(error);
      });
    } else {
      this.auditPlanService.auditPlansUpdateAuditPlan(auditPlanDetails, selectedEntityId).subscribe((result: string) => {
        // redirect to next section
        if (navigationPath !== null) {
          this.redirectToNextSection(navigationPath, result, sucessMessage, isFromAddPage);
        }
        }, error => {
          this.handleError(error);
        });
    }

  }

  /**
   * Redirect to next section page according to data passed
   * @param navigationPath : Path to redirect
   * @param auditPlanId : Audit plan id
   * @param sucessMessage : add/edit message
   * @param isFromAddPage : is from add page or edit page
   */
  redirectToNextSection(navigationPath: string, auditPlanId: string, sucessMessage: string, isFromAddPage: string) {
    this.sharedService.showSuccess(sucessMessage);
    if (navigationPath !== null) {
      this.router.navigate([navigationPath, { id: auditPlanId, isFromAdd: isFromAddPage }]);
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

    } else if (error.status === 404) {
      // for any unknow page request (page not found)
      this.router.navigate([this.stringConstants.pageNotFoundPath]);

    } else if (error.status === 401) {
      // for any unauthorized access
      this.router.navigate([this.stringConstants.unauthorizedPath]);

    } else {
      // check if duplicate entry exception then show error message otherwise show something went wrong message
      const errorMessage = error.error !== null ? error.error : this.stringConstants.somethingWentWrong;
      this.sharedService.showError(errorMessage);
    }
  }

  /**
   * Redirect to previous page
   * @param sectionType : Section type/ submenu type
   * @param auditplanId : Audit Plan Id
   * @param isFromAddPage : is from add page or edit page
   */
  redirectToPreviousPageSectionWise(sectionType: AuditPlanSectionType, auditplanId: string, isFromAddPage: string) {
    let navigationPath: string;

    switch (sectionType) {
      case AuditPlanSectionType.NUMBER_1:
        navigationPath = this.stringConstants.auditPlanGeneralPath;
        break;
      case AuditPlanSectionType.NUMBER_2:
        navigationPath = this.stringConstants.auditPlanOverviewPath;
        break;
      default:
        navigationPath = this.stringConstants.auditPlanPlanProcessPath;
        break;
    }
    this.router.navigate([navigationPath, { id: auditplanId, isFromAdd: isFromAddPage }]);
  }

  /**
   * Download paln document
   * @param planDocumentId : Id of the plan document
   * @param selectedEntityId : selected entity id
   */
  downloadPlanDocument(planDocumentId: string, selectedEntityId: string) {
    this.auditPlanService.auditPlansDownloadPlanDocument(planDocumentId, selectedEntityId).subscribe((result) => {
      const aTag = document.createElement('a');
      aTag.setAttribute('style', 'display:none;');
      document.body.appendChild(aTag);
      aTag.download = '';
      aTag.href = result;
      aTag.target = '_blank';
      aTag.click();
      document.body.removeChild(aTag);
    });
  }

  /**
   * Get file extention
   * @param fileName: file name
   */
  private getFileExtention(fileName: string) {
    return fileName.split('?')[0].split('.').pop().toLowerCase();
  }

  /**
   * Method to open document
   * @param documnetPath: documnetPath
   */
  openDocumentToView(documnetPath: string) {
    // Open document in new tab
    const url = this.router.serializeUrl(
      // Convert url to base64 encoding. ref https://stackoverflow.com/questions/41972330/base-64-encode-and-decode-a-string-in-angular-2
      this.router.createUrlTree(['document-preview/view', { path: btoa(documnetPath) }])
    );
    window.open(url, '_blank');
  }
}
