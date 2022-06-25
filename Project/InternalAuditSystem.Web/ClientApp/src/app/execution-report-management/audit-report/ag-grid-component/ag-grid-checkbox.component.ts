import { Component, OnInit } from '@angular/core';
import { AgRendererComponent } from 'ag-grid-angular';
import { IAfterGuiAttachedParams, AgGridEvent } from 'ag-grid-community';
import { ObservationAC } from '../../../swaggerapi/AngularFiles';
import { ReportSharedService } from '../report-shared.service';

@Component({
  selector: 'app-ag-grid-checkbox',
  templateUrl: './ag-grid-checkbox.component.html',
  styleUrls: ['./ag-grid.component.scss']

})
export class AgGridCheckboxComponent implements AgRendererComponent {

  constructor(private reportService: ReportSharedService) {  }

  params; // no type define as aggrid took any type of params
  observationList = [] as Array<ObservationAC>;

  /**
   * AG Grid life cycle init method
   * @param params:  inital value
   */
  // no type define as aggrid took any type of params
  agInit(params): void {
    this.params = params;
  }

  /**
   * UI attachment methods
   * @param params: IAfterGuiAttachedParams for GUI
   */
  afterGuiAttached(params?: IAfterGuiAttachedParams): void {
  }

  /**
   * AG render interface implemenation
   * @param params: selected check box value
   */
  // no type define as aggrid took any type of params
  refresh(params): boolean {
    params.api.refreshCells(params);
    if (params.value) {
      const observation = this.reportService.observationList.find(a => a.id === params.data.id);
      if (observation === undefined) {
        params.data.isSelected = true;
        this.reportService.observationList.push(params.data);
      } else {
        this.reportService.observationList.find(a => a.id === params.data.id).isSelected = true;
      }
    } else {
      this.reportService.observationList.find(a => a.id === params.data.id).isSelected = false;
    }
    return false;
  }
}
