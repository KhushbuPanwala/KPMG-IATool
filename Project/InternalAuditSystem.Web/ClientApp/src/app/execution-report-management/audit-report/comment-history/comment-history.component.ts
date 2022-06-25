import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { ActivatedRoute, Router } from '@angular/router';
import { ReportsService, ReportAC, ReportObservationAC } from '../../../swaggerapi/AngularFiles';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ReportCommentHistoryAC } from '../../../swaggerapi/AngularFiles/model/reportCommentHistoryAC';
import { ReportCommentAC } from '../../../swaggerapi/AngularFiles/model/reportCommentAC';
import { ReportObservationReviewerAC } from '../../../swaggerapi/AngularFiles/model/reportObservationReviewerAC';
import { SharedService } from '../../../core/shared.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-comment-history',
  templateUrl: './comment-history.component.html',
  styleUrls: ['./comment-history.component.scss']
})
export class CommentHistoryComponent implements OnInit, OnDestroy {
  commentHistoryTitle: string; // Variable for comment history page title
  backToolTip: string; // Variable for back tooltip
  reportTitleText: string; // Variable for report title
  observationTitleText: string; // Variable for report observation title
  commentsTitle: string; // Variable for comments title
  textAreaPlaceHolder: string; // Variable for text area placeholder
  authorName: string; // Variable for author name
  authorDate: string; // Variable for author date
  authorTime: string; // Variable for author time

  reportId; // current report id
  reportTitle: string;
  observationTitle: string;
  reportCommentList = {} as ReportCommentHistoryAC;
  commentList = [] as Array<ReportCommentAC>;
  reportObservationCommentList = [] as Array<ReportObservationReviewerAC>;
  reportObservationList = [] as Array<ReportObservationAC>;
  ObservationComment = [];
  observationCommentList = [];
  // only to subscripe for the current component
  entitySubscribe: Subscription;
  selectedEntityId: string;

  // HTML Editor
  config: AngularEditorConfig = {
    editable: false,
    spellcheck: true,
    height: '15rem',
    minHeight: '5rem',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
    toolbarHiddenButtons: [
      [
        'link',
        'unlink',
        'insertImage',
        'insertVideo',
        'toggleEditorMode',
        'undo',
        'redo',
        'removeFormat'
      ]
    ]
  };

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private apiService: ReportsService,
    public router: Router,
    private route: ActivatedRoute,
    private sharedService: SharedService
  ) {
    this.commentHistoryTitle = this.stringConstants.commentHistoryTitle;
    this.backToolTip = this.stringConstants.backToolTip;
    this.reportTitleText = this.stringConstants.reportTitle;
    this.observationTitleText = this.stringConstants.observationTitle;
    this.commentsTitle = this.stringConstants.commentsTitle;
    this.textAreaPlaceHolder = this.stringConstants.textAreaPlaceHolder;
    this.authorDate = this.stringConstants.authorDate;
    this.authorName = this.stringConstants.authorName;
    this.authorTime = this.stringConstants.authorTime;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.route.params.subscribe(params => {
          this.reportId = params.id;
          this.getCommentHistory();
        });
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
   * Get Comment history for report and report observation
   */
  getCommentHistory() {

    // get current system timezone
    const timeOffset = new Date().getTimezoneOffset();
    this.apiService.reportsGetCommentHistory(this.reportId, timeOffset, this.selectedEntityId).subscribe(result => {
      this.commentList = result.commentList;
      this.reportTitle = result.reportTitle;
      this.reportObservationList = result.reportObservationComment;
      for (const row of this.reportObservationList) {
        const groupObservations = new Set(row.observationReviewerList.map(item => item.reportObservationTitle));
        this.ObservationComment = [];
        groupObservations.forEach(g =>
          this.ObservationComment.push({
            name: g,
            values: row.observationReviewerList.filter(i => i.reportObservationTitle === g)
          }));
        this.observationCommentList.push(this.ObservationComment);
      }
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * on back button route to list page
   */
  onBackClick() {
    this.router.navigate(['report/list']);
  }

}
