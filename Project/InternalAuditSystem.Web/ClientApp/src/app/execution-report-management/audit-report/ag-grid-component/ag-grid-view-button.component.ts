import { Component, OnInit } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams, IAfterGuiAttachedParams } from 'ag-grid-community';
import { Router, ActivatedRoute } from '@angular/router';
import { StringConstants } from '../../../shared/stringConstants';

@Component({
  selector: 'app-ag-grid-view-button',
  templateUrl: './ag-grid-view-button.component.html',
  styleUrls: ['./ag-grid.component.scss']
})
export class AgGridViewButtonComponent implements ICellRendererAngularComp, OnInit {

  params;  // no type define as aggrid took any type of params
  selectedEntityId;
  reportId;
  constructor(private router: Router, private route: ActivatedRoute, private stringConstants: StringConstants) { }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
    this.route.params.subscribe(params => {
      this.reportId = params.id;
    });
  }


  /**
   * AG Grid life cycle init method
   * @param params: inital value
   */
  // no type define as aggrid took any type of params
  agInit(params): void {
    this.params = params;
  }

  /**
   * AG render interface implemenation
   * @param params: deleted observation value
   */
  // no type define as aggrid took any type of params
  refresh(params): boolean {
    params.api.refreshCells(params);
    this.router.navigate(['report/observation-add', { id: this.reportId, reportObservationId: params.value, type: this.stringConstants.viewOperationText }]);
    return false;
  }

}
