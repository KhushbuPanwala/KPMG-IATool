import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../shared/shared-bootstrap.module';
import { SharedModule } from '../shared/shared.module';
import { StrategicAnalysisSurveyUserRoutingModule} from './strategic-analysis-user-survey-routing.module';
import { StrategicAnalysisUserSurveyComponent } from './strategic-analysis-user-survey/strategic-analysis-user-survey.component';
import { StrategicAnalysisUserSurveySecondComponent } from './strategic-analysis-user-survey-second/strategic-analysis-user-survey-second.component';
import { StrategicAnalysisUserConfirmationComponent } from './strategic-analysis-user-confirmation/strategic-analysis-user-confirmation.component';
import { StrategicAnalysisUserEmailAttachmentComponent } from './strategic-analysis-user-email-attachment/strategic-analysis-user-email-attachment.component';



@NgModule({
  declarations: [StrategicAnalysisUserSurveyComponent, StrategicAnalysisUserSurveySecondComponent, StrategicAnalysisUserConfirmationComponent, StrategicAnalysisUserEmailAttachmentComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    StrategicAnalysisSurveyUserRoutingModule,
  ],
  providers: []

})
export class StrategicAnalysisUserSurveyModule { }
