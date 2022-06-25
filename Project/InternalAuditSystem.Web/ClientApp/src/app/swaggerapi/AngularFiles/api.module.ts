import { NgModule, ModuleWithProviders, SkipSelf, Optional } from '@angular/core';
import { Configuration } from './configuration';
import { HttpClient } from '@angular/common/http';


import { ACMReportsService } from './api/aCMReports.service';
import { AccountService } from './api/account.service';
import { AcmService } from './api/acm.service';
import { AuditCategoriesService } from './api/auditCategories.service';
import { AuditPlansService } from './api/auditPlans.service';
import { AuditProcessesService } from './api/auditProcesses.service';
import { AuditSubProcessesService } from './api/auditSubProcesses.service';
import { AuditTeamsService } from './api/auditTeams.service';
import { AuditTypesService } from './api/auditTypes.service';
import { AuditableEntitiesService } from './api/auditableEntities.service';
import { ClientParticipantsService } from './api/clientParticipants.service';
import { CountriesService } from './api/countries.service';
import { DistributorsService } from './api/distributors.service';
import { DivisionsService } from './api/divisions.service';
import { EntityCategoriesService } from './api/entityCategories.service';
import { EntityDocumentsService } from './api/entityDocuments.service';
import { EntityRelationMappingsService } from './api/entityRelationMappings.service';
import { EntityTypesService } from './api/entityTypes.service';
import { LocationsService } from './api/locations.service';
import { MomsService } from './api/moms.service';
import { ObservationCategoriesService } from './api/observationCategories.service';
import { ObservationsManagementService } from './api/observationsManagement.service';
import { PrimaryGeographicalAreasService } from './api/primaryGeographicalAreas.service';
import { RatingsService } from './api/ratings.service';
import { RcmProcessService } from './api/rcmProcess.service';
import { RcmSectorService } from './api/rcmSector.service';
import { RcmSubProcessService } from './api/rcmSubProcess.service';
import { RegionsService } from './api/regions.service';
import { RelationshipTypesService } from './api/relationshipTypes.service';
import { ReportDistributorsService } from './api/reportDistributors.service';
import { ReportObservationsService } from './api/reportObservations.service';
import { ReportsService } from './api/reports.service';
import { RiskAssessmentsService } from './api/riskAssessments.service';
import { RiskControlMatrixesService } from './api/riskControlMatrixes.service';
import { SamplingsService } from './api/samplings.service';
import { StatesService } from './api/states.service';
import { StrategicAnalysesService } from './api/strategicAnalyses.service';
import { UsersService } from './api/users.service';
import { WorkProgramsService } from './api/workPrograms.service';

@NgModule({
  imports:      [],
  declarations: [],
  exports:      [],
  providers: [
    ACMReportsService,
    AccountService,
    AcmService,
    AuditCategoriesService,
    AuditPlansService,
    AuditProcessesService,
    AuditSubProcessesService,
    AuditTeamsService,
    AuditTypesService,
    AuditableEntitiesService,
    ClientParticipantsService,
    CountriesService,
    DistributorsService,
    DivisionsService,
    EntityCategoriesService,
    EntityDocumentsService,
    EntityRelationMappingsService,
    EntityTypesService,
    LocationsService,
    MomsService,
    ObservationCategoriesService,
    ObservationsManagementService,
    PrimaryGeographicalAreasService,
    RatingsService,
    RcmProcessService,
    RcmSectorService,
    RcmSubProcessService,
    RegionsService,
    RelationshipTypesService,
    ReportDistributorsService,
    ReportObservationsService,
    ReportsService,
    RiskAssessmentsService,
    RiskControlMatrixesService,
    SamplingsService,
    StatesService,
    StrategicAnalysesService,
    UsersService,
    WorkProgramsService ]
})
export class ApiModule {
    public static forRoot(configurationFactory: () => Configuration): ModuleWithProviders {
        return {
            ngModule: ApiModule,
            providers: [ { provide: Configuration, useFactory: configurationFactory } ]
        };
    }

    constructor( @Optional() @SkipSelf() parentModule: ApiModule,
                 @Optional() http: HttpClient) {
        if (parentModule) {
            throw new Error('ApiModule is already loaded. Import in your base AppModule only.');
        }
        if (!http) {
            throw new Error('You need to import the HttpClientModule in your AppModule! \n' +
            'See also https://github.com/angular/angular/issues/20575');
        }
    }
}
