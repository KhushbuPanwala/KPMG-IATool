import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { StringConstants } from '../shared/stringConstants';
import { AuditableEntityAC, UserAC } from '../swaggerapi/AngularFiles';
import { saveAs } from 'file-saver';
import * as Excel from 'exceljs/dist/exceljs.min';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { LoaderService } from './loader.service';
import { LoggedInUserDetails } from '../swaggerapi/AngularFiles/model/loggedInUserDetails';

@Injectable()
export class SharedService {

  constructor(
    public toastr: ToastrService,
    public stringConstants: StringConstants,
    private router: Router,
    private loaderService: LoaderService) {
  }

  selectedEntitySubject = new BehaviorSubject<string>('');
  selectedEntityObjSubject = new BehaviorSubject<AuditableEntityAC>(null);
  selectionEntityListSubject = new BehaviorSubject<Array<AuditableEntityAC>>(null);
  currentUserDetailsSubject = new BehaviorSubject<LoggedInUserDetails>(null);
  onUpdateEntitySubject = new BehaviorSubject<LoggedInUserDetails>(null);

  /**
   * Update entityId whenever data changes for selection
   * @param entityId : Selected entity id
   */
   updateEntityId(entityId: string) {
     this.selectedEntitySubject.next(entityId);
   }

  /**
   * Update entityId whenever data changes for selection
   * @param entityObj : Selected entity
   */
  updateEntityObject(entityObj: AuditableEntityAC) {
    this.selectedEntityObjSubject.next(entityObj);
  }

  /**
   * Update current user details on login
   * @param userDetails : new updated list
   */
  updateCurrentUserDetails(userDetails: LoggedInUserDetails) {
    this.currentUserDetailsSubject.next(userDetails);
  }

  /**
   * Update current user and entity list on entity list update
   * @param userDetails : new updated list
   */
  updateOnEntityListUpdates(userDetails: LoggedInUserDetails) {
    this.onUpdateEntitySubject.next(userDetails);
  }

  /**
   * Retrict access if entity not selected
   */
  retrictAccess() {
    this.router.navigate(['']);
    this.toastr.warning('Please select entity to access this section');
  }

  /**
   * Set showing result data
   * @param pageNumber:  current page no.
   * @param selectedPageItem: items per page
   * @param totalRecords: total no. of reecords
   */
  setShowingResult(pageNumber: number, selectedPageItem: number, totalRecords: number) {
    const startItem = (pageNumber - 1) * selectedPageItem + 1;
    const endItem = (totalRecords < (pageNumber * selectedPageItem)) ? totalRecords : (pageNumber * selectedPageItem);
    const showingResults = `${this.stringConstants.showingText} ${startItem} - ${endItem} ${this.stringConstants.ofText} ${totalRecords}`;
    return showingResults;
  }

  /**
   * Show info message toaster
   * @param infoMsg: Information message
   */
  showInfo(infoMsg: string) {
    this.toastr.info(infoMsg, '', { timeOut: 5000 });
  }

  /**
   * Show warning message toaster
   * @param wardingMsg: Information message
   */
  showWarning(warningMsg: string) {
    this.toastr.warning(warningMsg, '', { timeOut: 5000 });
  }

  /**
   * Show error message toaster
   * @param errorMsg: Error message
   */
  showError(errorMsg: string) {
    this.toastr.error(errorMsg, '', { timeOut: 5000 });
  }

  /**
   * Show sucess message toaster
   * @param successMsg: Success message
   */
  showSuccess(successMsg: string) {
    this.toastr.success(successMsg, '', { timeOut: 5000 });
  }

  /**
   * Method for converting Utc to local datetime
   * @param date : Covert datetime by GMT offset
   * @param toUTC : If toUTC is true then return UTC time other wise return local time
   */
  convertLocalDateToUTCDate(date, toUTC: boolean) {
    date = new Date(date);

    // Local time converted to UTC
    const localOffset = date.getTimezoneOffset() * 60000;
    const localTime = date.getTime();
    if (toUTC) {
      date = localTime + localOffset;
    } else {
      date = localTime - localOffset;
    }
    date = new Date(date);
    return date;
  }

  /**
   * Export data to excel
   * @param url: api url for export data
   */
  exportToExcel(url: string) {
    // create dynamic link for export file download
    const link = document.createElement('a');
    link.style.display = 'none';
    link.setAttribute('href', url);
    link.click();
    this.showSuccess(this.stringConstants.recordExportedMsg);
  }

  /**
   * Method for generating pdf
   * @param url: api url for generating pdf
   */
  generatePdf(url: string) {
    // create dynamic link for export file download
    const link = document.createElement('a');
    link.style.display = 'none';
    link.setAttribute('href', url);
    link.click();
    this.showSuccess(this.stringConstants.recordPdfMsg);
  }

  /**
   * Create PPT Report
   * @param url: api url for export data
   */
  createPPT(url: string) {
    // create dynamic link for export file download
    const link = document.createElement('a');
    link.style.display = 'none';
    link.setAttribute('href', url);
    link.click();
    this.showSuccess(this.stringConstants.recordPPTdMsg);
  }

  /**
   * Create Bulk Upload Excel template
   */
  createBulkUploadExcelTemplate() {
    const workBook = new Excel.Workbook();
    workBook.views = [
      {
        x: 0, y: 0, width: 10000, height: 20000,
        firstSheet: 0, activeTab: 1, visibility: 'visible'
      }
    ];
    return workBook;
  }

  /**
   * CreaDownload te Bulk Upload template
   * @param fileName: template file name
   * @param workBook: current workbook
   */
  downloadBulkUploadTemplate(fileName: string, workBook) {
    workBook.xlsx.writeBuffer(workBook).then(function(buffer) {
      const blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64' });
      saveAs(blob, fileName);
    });
  }

  /**
   * Handle error scenario in case of add/ update
   * @param error : http error
   */
  handleError(error: HttpErrorResponse) {
    this.loaderService.close();

    // if entity close then show info of close status (ForBidden)
    if (error.status === 403) {
      this.showInfo(this.stringConstants.closedEntityRestrictionMessage);

    } else if (error.status === 405) {
      // if entity is deleted then show warning for the action (not allowed)
      this.showWarning(this.stringConstants.deletedEntityRestrictionMessage);

    } else if (error.status === 404) {
      // for any unknow page request (page not found)
      this.router.navigate([this.stringConstants.pageNotFoundPath]);

    } else if (error.status === 401) {
      // for any unauthorized access
      this.router.navigate([this.stringConstants.unauthorizedPath]);

    } else {
      // check if duplicate entry exception then show error message otherwise show something went wrong message
      const errorMessage = error.error !== null ? error.error : this.stringConstants.somethingWentWrong;
      this.showError(errorMessage);
    }
  }
}
