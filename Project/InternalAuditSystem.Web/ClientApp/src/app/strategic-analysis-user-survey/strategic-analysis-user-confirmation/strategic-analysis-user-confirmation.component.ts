import { Component, OnInit } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../shared/stringConstants';
import { StrategicAnalysisUserSurveySecondComponent } from '../strategic-analysis-user-survey-second/strategic-analysis-user-survey-second.component';
import { ActivatedRoute, Router } from '@angular/router';
import { StrategicAnalysesService } from '../../swaggerapi/AngularFiles';
import { ToastrService } from 'ngx-toastr';
import { LoaderService } from '../../core/loader.service';
import { SharedService } from '../../core/shared.service';

@Component({
  selector: 'app-strategic-analysis-user-confirmation',
  templateUrl: './strategic-analysis-user-confirmation.component.html'
})
export class StrategicAnalysisUserConfirmationComponent implements OnInit {

  strategicConfimMessage: string; // Variable for strategic confirmation message
  yes: string; // Variable for yes
  no: string; // Variable for no
  strategicId: string;

  // Creates an instance of documenter
  constructor(public bsModalRef: BsModalRef,
              private modalService: BsModalService,
              private stringConstants: StringConstants,
              private route: ActivatedRoute,
              private apiService: StrategicAnalysesService,
              private router: Router,
              private toastr: ToastrService,
              private loaderService: LoaderService,
              private sharedService: SharedService) {

    this.strategicConfimMessage = this.stringConstants.strategicConfimMessage;
    this.yes = this.stringConstants.yes;
    this.no = this.stringConstants.no;
  }

/*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
 *  Initialization of properties.
 */
  ngOnInit(): void {
  }
  onYes() {
    if (this.bsModalRef.content.callback != null) {
      this.bsModalRef.content.callback(this.stringConstants.yes);
      this.bsModalRef.hide();
    }
  }
  onNo() {
    if (this.bsModalRef.content.callback != null) {
      this.bsModalRef.content.callback(this.stringConstants.no);
      this.bsModalRef.hide();
    }
  }
}
