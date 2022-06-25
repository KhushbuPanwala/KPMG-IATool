import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PageNotFoundComponent } from './shared/page-not-found/page-not-found.component';
import { UnAuthorizedComponent } from './shared/unauthorized-page/unauthorized.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: '',
    pathMatch: 'full'
  },
  {
    path: 'sampling', loadChildren: () => import('./sampling/sampling.module').then(m => m.SamplingModule),
    data: { breadCrumb: 'Home > Sampling' }
  },
  {
    path: 'mom',
    loadChildren: () => import('./mom/mom.module').then(m => m.MomModule),
    data: { breadCrumb: 'Execution And Reporting >  Master >   MOM' }
  },
  {
    path: 'rating',
    loadChildren: () => import('./execution-report-management/masters/rating/rating.module').then(m => m.RatingModule),
    data: { breadCrumb: 'Execution And Reporting >  Master >   Rating' }
  },
  {
    path: 'report', loadChildren: () => import('./execution-report-management/audit-report/report.module').then(m => m.ReportModule),
    data: { breadCrumb: 'Execution And Reporting > Report Management >  Audit Report' }
  },
  {
    path: 'distribution', loadChildren: () => import('./execution-report-management/distribution/distribution.module').then(m => m.DistributionModule),
    data: { breadCrumb: 'Execution And Reporting  > Report Management > Distribution List' }
  },
  {
    path: 'process', loadChildren: () => import('./rcm/rcm-process/process.module').then(m => m.ProcessModule),
    data: { breadCrumb: 'RCM > Master > RCM Process' }
  },
  {
    path: 'sector', loadChildren: () => import('./rcm/rcm-sector/sector.module').then(m => m.SectorModule),
    data: { breadCrumb: 'RCM > Master > Sector' }
  },
  {
    path: 'rcm', loadChildren: () => import('./rcm/rcm-main/rcm.module').then(m => m.RcmModule),
    data: { breadCrumb: 'RCM > RCM ' }
  },
  {
    path: 'audit-team', loadChildren: () => import('./auditable-entity/masters/audit-team/audit-team.module').then(m => m.AuditTeamModule),
    data: { breadCrumb: 'Auditable Entity > Masters > Audit Team' }
  },
  {
    path: 'rcm-sub-process', loadChildren: () => import(
      './rcm/rcm-sub-process/rcm-sub-process/rcm-sub-process.module')
      .then(m => m.RcmSubProcessModule),
    data: { breadCrumb: 'RCM > Master > RCM Process' }
  },
  {
    path: 'client-participants', loadChildren: () =>
      import('./auditable-entity/masters/client-participants/client-participants.module')
        .then(m => m.ClientParticipantsModule),
    data: { breadCrumb: 'Auditable Entity > Masters > Client Participant' }
  },
  {
    path: 'work-program', loadChildren: () =>
      import('./work-program/work-program.module')
        .then(m => m.WorkProgramModule),
    data: { breadCrumb: 'Audit Plan > Audit Work Program > Work Program' }
  },
  {
    path: 'audit-type', loadChildren: () =>
      import('./audit-plan/masters/audit-type/audit-type.module')
        .then(m => m.AuditTypeModule),
    data: { breadCrumb: 'Audit Plan > Master > Audit Type' }
  },
  {
    path: 'audit-category', loadChildren: () =>
      import('./audit-plan/masters/audit-category/audit-category.module')
        .then(m => m.AuditCategoryModule),
    data: { breadCrumb: 'Audit Plan > Master > Audit Category' }
  },
  {
    path: 'strategic-analysis',
    loadChildren: () => import('./strategic-analysis-admin/strategic-analysis-admin.module').then(m => m.StrategicAnalysisAdminModule),
    data: { breadCrumb: 'Home > Strategic Analysis' }
  },
  {
    path: 'audit-sub-process', loadChildren: () =>
      import('./audit-plan/masters/audit-sub-process/audit-sub-process.module')
        .then(m => m.AuditSubProcessModule),
    data: { breadCrumb: 'Audit Plan > Master > Audit Sub Process' }
  },
  {
    path: 'audit-process', loadChildren: () =>
      import('./audit-plan/masters/audit-process/audit-process.module')
        .then(m => m.AuditProcessModule),
    data: { breadCrumb: 'Audit Plan > Master > Audit Process' }
  },

  {
    path: 'strategic-analysis-survey', loadChildren: () =>
      import('./strategic-analysis-admin-survey/strategic-analysis-admin-survey.module')
        .then(m => m.StrategicAnalysisAdminSurveyModule),
    data: { breadCrumb: 'Home > Strategic Analysis > Survey' }
  },

  {
    path: 'auditable-entity-type', loadChildren: () =>
      import('./auditable-entity/masters/auditable-entity-type/auditable-entity-type.module')
        .then(m => m.AuditableEntityTypeModule),
    data: { breadCrumb: 'Auditibale Entity > Masters > Auditibale Entity Type' }
  },

  {
    path: 'auditable-entity-category', loadChildren: () =>
      import('./auditable-entity/masters/auditable-entity-category/auditable-entity-category.module')
        .then(m => m.AuditableEntityCategoryModule),
    data: { breadCrumb: 'Auditibale Entity > Masters > Auditibale Entity Sector' }
  },
  {
    path: 'strategic-analysis-response', loadChildren: () =>
      import('./strategic-analysis-admin-response/strategic-analysis-admin-response.module')
        .then(m => m.StrategicAnalysisAdminResponseModule),
    data: { breadCrumb: 'Home > Strategic Analysis > Response' }
  },
  {
    path: 'strategic-analysis-user', loadChildren: () =>
      import('./strategic-analysis-user/strategic-analysis-user.module')
        .then(m => m.StrategicAnalysisUserModule),
    data: { breadCrumb: 'Home > Strategic Analysis' }
  },
  {
    path: 'strategic-analysis-user-survey', loadChildren: () =>
      import('./strategic-analysis-user-survey/strategic-analysis-user-survey.module')
        .then(m => m.StrategicAnalysisUserSurveyModule),
    data: { breadCrumb: 'Home > Strategic Analysis > Survey' }
  },
  {
    path: 'relationship-type', loadChildren: () => import('./auditable-entity/masters/relationship-type/relationship-type.module').then(m => m.RelationshipTypeModule),
    data: { breadCrumb: 'Auditable Entity > Masters > Relationship Type' }
  },
  {
    path: 'division', loadChildren: () => import('./auditable-entity/masters/division/division.module').then(m => m.DivisionModule),
    data: { breadCrumb: 'Auditable Entity > Masters > Division' }
  },
  {
    path: 'location', loadChildren: () => import('./auditable-entity/masters/location/location.module').then(m => m.LocationModule),
    data: { breadCrumb: 'Auditable Entity > Masters > Location' }
  },
  {
    path: 'auditable-entity', loadChildren: () => import('./auditable-entity/auditable-entity-list/auditable-entity-list.module').then(m => m.AuditableEntityListModule),
    data: { breadCrumb: 'Auditable Entity > Auditable Entity' }
  },
  // audit plan routings & bread crumbs
  {
    path: 'audit-plan', loadChildren: () => import('./audit-plan/audit-plan-list/audit-plan-list.module').then(m => m.AuditPlanModule),
    data: { breadCrumb: 'Audit Plan > Audit Plan' }
  },
  {
    path: 'audit-plan', loadChildren: () => import('./audit-plan/audit-plan-general/audit-plan-general.module').then(m => m.AuditGeneralModule),
    data: { breadCrumb: 'Audit Plan > Audit Plan > General' }
  },
  {
    path: 'audit-plan', loadChildren: () => import('./audit-plan/audit-plan-overview/audit-plan-overview.module').then(m => m.AuditPlanOverviewModule),

    data: { breadCrumb: 'Audit Plan > Audit Plan > Overview' }
  },
  {
    path: 'audit-plan', loadChildren: () => import('./audit-plan/plan-audit-process/plan-audit-process.module').then(m => m.PlanAuditProcessModule),
    data: { breadCrumb: 'Audit Plan > Audit Plan > Plan Processes' }
  },
  {
    path: 'audit-plan', loadChildren: () => import('./audit-plan/plan-document/plan-document.module').then(m => m.PlanDocumentModule),
    data: { breadCrumb: 'Audit Plan > Audit Plan > Documents' }
  },
  {
    path: 'observation-management', loadChildren: () => import('./observation/observation.module').then(m => m.ObservationModule),
    data: { breadCrumb: 'Execution And Reporting > Observation Management' }
  },

  {
    path: 'region', loadChildren: () => import('./auditable-entity/masters/region/region.module').then(m => m.RegionModule),
    data: { breadCrumb: 'Auditable Entity > Masters > Region' }
  },

  {
    path: 'country', loadChildren: () => import('./auditable-entity/masters/country/country.module').then(m => m.CountryModule),
    data: { breadCrumb: 'Auditable Entity > Masters > Country' }
  },
  {
    path: 'state', loadChildren: () => import('./auditable-entity/masters/state/state.module').then(m => m.StateModule),
    data: { breadCrumb: 'Auditable Entity > Masters > Province/State' }
  },
  {
    path: 'auditable-entity', loadChildren: () => import('./auditable-entity/risk-assesment/risk-assesment.module')
      .then(m => m.RiskAssesmentModule),
    data: { breadCrumb: 'Auditable Entity > Risk Assessment Details' }
  },
  {
    path: 'auditable-entity', loadChildren: () => import('./auditable-entity/relationship/relationship.module').then(m => m.RelationshipModule),

    data: { breadCrumb: 'Auditable Entity > Relationship' }
  },
  {
    path: 'auditable-entity', loadChildren: () => import('./auditable-entity/entity-documents/entity-documents.module').then(m => m.EntityDocumentsModule),

    data: { breadCrumb: 'Auditable Entity > Documents' }
  },
  {
    path: 'auditable-entity', loadChildren: () => import('./auditable-entity/geographical-area/geographical-area.module').then(m => m.GeographicalAreaModule),
    data: { breadCrumb: 'Auditable Entity > Primary Geographical Areas' }
  },
  {
    path: 'auditable-entity', loadChildren: () => import('./auditable-entity/auditable-entity-detail/auditable-entity-detail.module').then(m => m.AuditableEntityDetailModule),
    data: { breadCrumb: 'Auditable Entity > Details' }
  },
  {
    path: 'auditable-entity', loadChildren: () => import('./auditable-entity/classification/classification.module').then(m => m.ClassificationModule),
    data: { breadCrumb: 'Auditable Entity > Classification' }
  },
  {
    path: 'acm', loadChildren: () => import('./acm/acm.module').then(m => m.AcmModule),
    data: { breadCrumb: 'Execution and Reporting > ACM Presentation' }
  },
  {
    path: 'audit-advisory', loadChildren: () => import('./audit-advisory/audit-advisory.module').then(m => m.AuditAdvisoryModule),
    data: { breadCrumb: 'Auditable Entity > Audit Advisory' }
  },

  {
    path: 'document-preview', loadChildren: () => import('./document-preview/document-preview.module').then(m => m.DocumentPreviewModule),
    data: { breadCrumb: 'Document Preview' }
  },
  {
    path: 'observation-category', loadChildren: () => import('./execution-report-management/masters/observation-category/observation-category.module').then(m => m.ObservationCategoryModule),
    data: { breadCrumb: 'Execution and Reporting > Master > Observation Category' }
  },
  {
    path: '404',
    component: PageNotFoundComponent
  },
  {
    path: '401',
    component: UnAuthorizedComponent
  },
  {
    path: '**',
    redirectTo: '404'
  },
];


@NgModule({
  imports: [
    RouterModule.forRoot(routes, { onSameUrlNavigation: 'reload' })
  ],

  exports: [RouterModule],
  providers: []
})
export class AppRoutingModule {
}
