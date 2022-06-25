import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation, OnDestroy } from '@angular/core';
import { setTheme } from 'ngx-bootstrap/utils';
import { NavItem } from './sidebar-item';
import { StringConstants } from '../app/shared/stringConstants';
import { SharedService } from './core/shared.service';
import { AuditableEntitiesService, AuditableEntityAC, UserAC, UsersService, UserRole, UserType } from './swaggerapi/AngularFiles';
import { LoaderService } from './core/loader.service';
import { ActivatedRoute, Router } from '@angular/router';
import { LoggedInUserDetails } from './swaggerapi/AngularFiles/model/loggedInUserDetails';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  encapsulation: ViewEncapsulation.None
})

export class AppComponent implements OnInit, OnDestroy {
  toggleSideBar: boolean; // Variable to toggle sidebar
  selectedItem = 0;
  dashboardBreadCrumbTitle: string; // Variable to display dashboard bredcrumb title
  masterBreadCrumbTitle: string; // Variable to display master bredcrumb title
  auditPlanBreadCrumbTitle: string;
  copyRightTitle: string; // Variable to display master bredcrumb title
  entitySelectionPlaceHolder: string;
  version: string;
  entitySelectionLabel: string;
  selectedEntity: string;
  loggedInUser: string;

  // objects
  selectedEntityObj = {} as AuditableEntityAC;
  auditableEntityList = [] as Array<AuditableEntityAC>;
  currentUserDetails = {} as UserAC;
  finalMenuItem: NavItem[] = [];

  // only to subscripe for the current component
  userSubscribe: Subscription;

  strategicAdmin: NavItem = {
    displayName: 'Strategic Analysis',
    iconName: 'zmdi-book',
    route: 'strategic-analysis/list',
  };

  strategicUser: NavItem = {
    displayName: 'Strategic Analysis',
    iconName: 'zmdi-book',
    route: 'strategic-analysis-user/list',
  };

  samplingAdminMenu: NavItem = {
    displayName: 'Sampling',
    iconName: 'zmdi-lamp',
    route: 'sampling/list',
    children: []
  };

  // basic menu items to display
  defaultMenuItem: NavItem[] = [
    {
      displayName: 'Dashbord',
      iconName: 'zmdi-home',
    },
    {
      displayName: 'Strategic Analysis',
      iconName: 'zmdi-book',
      route: ' '
    },
    {
      displayName: 'Sampling',
      iconName: 'zmdi-lamp',
      route: 'sampling/list',
      children: []
    },
  ];

  // entity wise menu items to display
  entityWiseNavItem: NavItem[] = [
    {
      displayName: 'Dashbord',
      iconName: 'zmdi-home',
    },
    {
      displayName: 'Strategic Analysis',
      iconName: 'zmdi-book',
      route: ' ',
      children: []
    },
    {
      displayName: 'Sampling',
      iconName: 'zmdi-lamp',
      route: 'sampling/list',
      children: []
    },
    {
      displayName: 'RCM',
      iconName: 'zmdi-assignment-check',
      route: 'rcm',
      children: [

        {
          displayName: 'Masters',
          iconName: 'none',
          route: 'masters',
          children: [
            {
              displayName: 'Sector',
              iconName: 'none',
              route: 'sector/list',
            },
            {
              displayName: 'RCM Process',
              iconName: 'none',
              route: 'process/list',
            },
            {
              displayName: 'RCM Sub Process',
              iconName: 'none',
              route: 'rcm-sub-process/list'
            }
          ]
        },
        {
          displayName: 'RCM',
          iconName: 'none',
          route: 'rcm/list',
        }
      ]
    },
    {
      displayName: 'Auditable Entity',
      iconName: 'zmdi-chart',
      route: ' ',
      children: [
        {
          displayName: 'Masters',
          iconName: 'group',
          children: [
            {
              displayName: 'Auditable Entity Type',
              iconName: 'person',
              route: 'auditable-entity-type/list'
            },
            {
              displayName: 'Auditable Entity Sector',
              iconName: 'person',
              route: 'auditable-entity-category/list'
            },
            {
              displayName: 'Division',
              iconName: 'person',
              route: 'division/list'
            },
            {
              displayName: 'Relationship Type',
              iconName: 'person',
              route: 'relationship-type/list'
            },
            {
              displayName: 'Region',
              iconName: 'person',
              route: 'region/list'
            },
            {
              displayName: 'Country',
              iconName: 'person',
              route: 'country/list'
            },
            {
              displayName: 'Province/state',
              iconName: 'person',
              route: 'state/list'
            },
            {
              displayName: 'Location',
              iconName: 'person',
              route: 'location/list'
            },
            {
              displayName: 'Audit Team',
              iconName: 'person',
              route: 'audit-team/list'
            },
            {
              displayName: 'Client Participants',
              iconName: 'person',
              route: 'client-participants/list'
            }
          ]
        },
        {
          displayName: 'Auditable Entity',
          iconName: 'none',
          route: 'auditable-entity/list'
        },
        {
          displayName: 'Audit Advisory',
          iconName: 'none',
          route: 'audit-advisory/list'
        },

      ]
    },
    {
      displayName: 'Audit Plan',
      iconName: 'zmdi-library',
      route: ' ',
      children: [
        {
          displayName: 'Audit Plan',
          iconName: 'none',
          route: 'audit-plan/list'
        },
        {
          displayName: 'Masters',
          iconName: 'none',
          children: [
            {
              displayName: 'Audit Type',
              iconName: 'none',
              route: 'audit-type/list',
            },
            {
              displayName: 'Audit Category',
              iconName: 'none',
              route: 'audit-category/list',
            },
            {
              displayName: 'Audit Process',
              iconName: 'none',
              route: 'audit-process/list',
            },
            {
              displayName: 'Audit Sub Process',
              iconName: 'none',
              route: 'audit-sub-process/list',
            }
          ]
        },
        {
          displayName: 'Audit Work Program',
          iconName: 'none',
          children: [
            {
              displayName: 'Work Program',
              iconName: 'none',
              route: 'work-program/list'
            }
          ]
        }
      ]
    },
    {
      displayName: 'Execution and Reporting',
      iconName: 'zmdi-case',
      route: 'execution-reporting',
      children: [
        {
          displayName: 'Masters',
          iconName: 'none',
          route: 'masters',
          children: [
            {
              displayName: 'Audit Rating',
              iconName: 'none',
              route: '/rating/list',
            },
            {
              displayName: 'Observation Category',
              iconName: 'none',
              route: '/observation-category/list',
            }
          ]
        },
        {
          displayName: 'Observation Management',
          iconName: 'none',
          route: 'observation-management/list',
        },
        {
          displayName: 'Minutes of Meeting',
          iconName: 'none',
          route: '/mom/list',
        },
        {
          displayName: 'Report Management',
          iconName: 'none',
          route: 'report-management',
          children: [

            {
              displayName: 'Audit Report',
              iconName: 'none',
              route: '/report/list',
            },
            {
              displayName: 'Distribution List',
              iconName: 'none',
              route: '/distribution/list',
            }
          ]
        },
        {
          displayName: 'ACM Presentation',
          iconName: 'none',
          route: 'acm/list',
        }
      ]
    },
    {
      displayName: 'Settings',
      iconName: 'zmdi-settings'
    }
  ];

  mainRoutePath: NavItem[] = [
    {
      mainRouteName: '',
      route: '/',
    },
    {
      mainRouteName: 'sampling',
      route: '/sampling/list',
    },
    {
      mainRouteName: 'sector',
      route: '/sector/list',
    },
    {
      mainRouteName: 'process',
      route: '/process/list',
    },
    {
      mainRouteName: 'rcm-sub-process',
      route: '/rcm-sub-process/list',
    },
    {
      mainRouteName: 'rcm',
      route: '/rcm/list',
    },
    {
      mainRouteName: 'auditable-entity',
      route: '/auditable-entity/list',
    },
    {
      mainRouteName: 'audit-advisory',
      route: '/audit-advisory/list',
    },
    {
      mainRouteName: 'auditable-entity-type',
      route: '/auditable-entity-type/list',
    },
    {
      mainRouteName: 'auditable-entity-category',
      route: '/auditable-entity-category/list',
    },
    {
      mainRouteName: 'division',
      route: '/division/list',
    },
    {
      mainRouteName: 'relationship-type',
      route: '/relationship-type/list',
    },
    {
      mainRouteName: 'region',
      route: '/region/list',
    },
    {
      mainRouteName: 'country',
      route: '/country/list',
    },
    {
      mainRouteName: 'state',
      route: '/state/list',
    },
    {
      mainRouteName: 'location',
      route: '/location/list',
    },
    {
      mainRouteName: 'audit-team',
      route: '/audit-team/list',
    },
    {
      mainRouteName: 'client-participants',
      route: '/client-participants/list',
    },
    {
      mainRouteName: 'audit-advisory',
      route: '/audit-advisory/list',
    },
    {
      mainRouteName: 'rating',
      route: '/rating/list',
    },
    {
      mainRouteName: 'observation-category',
      route: '/observation-category/list',
    },
    {
      mainRouteName: 'mom',
      route: '/mom/list',
    },
    {
      mainRouteName: 'report',
      route: '/report/list',
    },
    {
      mainRouteName: 'distribution',
      route: '/distribution/list',
    },
    {
      mainRouteName: 'acm',
      route: '/acm/list',
    },
    {
      mainRouteName: 'audit-plan',
      route: '/audit-plan/list',
    },
    {
      mainRouteName: 'audit-category',
      route: '/audit-category/list',
    },
    {
      mainRouteName: 'audit-type',
      route: '/audit-type/list',
    },
    {
      mainRouteName: 'audit-process',
      route: '/audit-process/list',
    },
    {
      mainRouteName: 'audit-sub-process',
      route: '/audit-sub-process/list',
    },
    {
      mainRouteName: 'observation-management',
      route: '/observation-management/list',
    },
    {
      mainRouteName: 'work-program',
      route: '/work-program/list',
    },
  ];
  constructor(
    private stringConstants: StringConstants,
    private sharedService: SharedService,
    private auditableEntityService: AuditableEntitiesService,
    private loaderService: LoaderService,
    private userService: UsersService,
    private router: Router,
    private activeRoute: ActivatedRoute) {

    // Set bootstrap theme
    setTheme('bs4');

    // String constants declarations
    this.dashboardBreadCrumbTitle = this.stringConstants.dashboardBreadCrumbTitle;
    this.masterBreadCrumbTitle = this.stringConstants.masterBreadCrumbTitle;
    this.auditPlanBreadCrumbTitle = this.stringConstants.auditPlanBreadCrumbTitle;
    this.copyRightTitle = this.stringConstants.copyRightTitle;
    this.entitySelectionPlaceHolder = this.stringConstants.entitySelectionPlaceHolder;
    this.version = this.stringConstants.versionTitle;
    this.entitySelectionLabel = this.stringConstants.auditableEntityTitle;
    this.loggedInUser = this.stringConstants.loggedInUser;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
    this.loaderService.open();
    // get the default selected id and kepe it as observable
    this.auditableEntityService.auditableEntitiesGetPermittedEntitiesOfLoggedInUser().subscribe((loggedInUserDetails: LoggedInUserDetails) => {

      this.userSubscribe = this.sharedService.onUpdateEntitySubject.subscribe((currentUser: LoggedInUserDetails) => {

        this.auditableEntityList = currentUser !== null ? JSON.parse(JSON.stringify(currentUser.permittedEntityList)) : JSON.parse(JSON.stringify(loggedInUserDetails.permittedEntityList));

        // current user details
        this.currentUserDetails = currentUser !== null ? JSON.parse(JSON.stringify(currentUser.userDetails)) : JSON.parse(JSON.stringify(loggedInUserDetails.userDetails));

        const userdetails = currentUser !== null ? currentUser : loggedInUserDetails;
        this.sharedService.updateCurrentUserDetails(userdetails);

        // update selected entity as per loggedin user details
        if (this.currentUserDetails.currentSelectedEntityId !== null) {
          this.populateDisplayEntityWise(this.currentUserDetails.currentSelectedEntityId, true);
        } else {
          this.loaderService.close();
          this.router.navigate(['']);
          this.selectedEntity = undefined;
          this.entitySelectionPlaceHolder = this.stringConstants.entitySelectionPlaceHolder;
          // update auditable  entity id
          this.sharedService.updateEntityId('');
          // update  auditable entity as subject
          this.sharedService.updateEntityObject({} as AuditableEntityAC);
          // sampling menu
          if (this.currentUserDetails.userRole === UserRole.NUMBER_2) {
            this.defaultMenuItem.splice(2, 1);
          }
          this.finalMenuItem = JSON.parse(JSON.stringify(this.defaultMenuItem));
          this.setStrategicAnalysisMenu();
        }
      });

    });
  }

  /**
   * A lifecycle hook that is called when a directive, pipe, or service is destroyed.
   * Use for any custom cleanup that needs to occur when the instance is destroyed.
   */
  ngOnDestroy() {
    this.userSubscribe.unsubscribe();
  }

  /**
   * Populate entity wise display in side menu
   * @param selectedEntityId : id of the selected entity from upper side of display
   */
  updateSelectedEntity(selectedEntityId: string) {
    this.loaderService.open();

    // assign selected entity for logged in user
    const user = {} as UserAC;
    user.currentSelectedEntityId = selectedEntityId;
    this.userService.usersUpdateSelectedEntityForCurrentUser(user).subscribe(() => {
      this.loaderService.close();
      this.populateDisplayEntityWise(selectedEntityId, false);
    });
  }

  /**
   * Update selected entity details for every component
   * @param selectedEntityId : id of the selected entity from upper side of display
   * @param isOnPageRefresh : boolean value to check is called on page refresh or not
   */
  async populateDisplayEntityWise(entityId: string, isOnPageRefresh: boolean) {
    if (entityId === undefined) {
      entityId = null;
    }
    if (entityId !== null) {
      if (this.currentUserDetails.userRole === UserRole.NUMBER_2) {
        // check if menu present in list or not if present then only remove
        const index = this.entityWiseNavItem.findIndex(x => x.displayName === 'Sampling');
        if (index !== -1) {
          this.entityWiseNavItem.splice(index, 1);
        }
      }
      this.finalMenuItem = JSON.parse(JSON.stringify(this.entityWiseNavItem));
      this.selectedEntityObj = this.auditableEntityList.find(x => x.id === entityId);
      this.selectedEntity = entityId;
      if (!isOnPageRefresh) {
        await this.changeRouteOnEntityChange();
      }
      // update auditable  entity id
      this.sharedService.updateEntityId(this.selectedEntity);
      // update  auditable entity as subject
      this.sharedService.updateEntityObject(this.selectedEntityObj);
    } else {

      // sampling menu
      if (this.currentUserDetails.userRole === UserRole.NUMBER_2) {
        this.defaultMenuItem.splice(2, 1);
      }
      this.finalMenuItem = JSON.parse(JSON.stringify(this.defaultMenuItem));
    }
    this.setStrategicAnalysisMenu();
    this.loaderService.close();
  }

  /**
   * Open main menu item
   */
  openMenuItem() {
    this.toggleSideBar = !this.toggleSideBar;
  }

  /**
   * Change route according on entity change
   */
  async changeRouteOnEntityChange() {
    const currentPath = location.pathname;
    let navigationPath = currentPath;
    const mainModule = this.mainRoutePath.find(x => x.route === currentPath);

    // ignore if current route is of strategy analysis
    if (!currentPath.includes(this.stringConstants.strategicRoutePathName)) {

      // if current path is other than defined module path then redirect to list page
      if (mainModule === undefined) {
        const currentModuleRoute = currentPath.substring(currentPath.indexOf('/'), currentPath.indexOf(';'));
        // get main module route
        const mainRoute = currentModuleRoute.substring(currentModuleRoute.indexOf('/') + 1, currentModuleRoute.lastIndexOf('/'));
        navigationPath = this.mainRoutePath.find(x => x.mainRouteName === mainRoute).route;
      }
      this.router.navigate([navigationPath]);
    }
  }

  /**
   * Set strategic analysis menu according to logged user type
   */
  setStrategicAnalysisMenu() {
    const currentPath = location.pathname;
    const strategyIndex = this.finalMenuItem.findIndex(x => x.displayName === this.stringConstants.strategicAnalysis);
    const samplingIndex = this.finalMenuItem.findIndex(x => x.displayName === this.stringConstants.samplingModuleTitle);

    if (this.currentUserDetails.userRole === UserRole.NUMBER_0 || this.currentUserDetails.userRole === UserRole.NUMBER_1) {
      if (currentPath.includes(this.stringConstants.strategicRoutePathName) && location.pathname.includes('user')) {
        this.router.navigate([this.stringConstants.unauthorizedPath]);
      }
      this.finalMenuItem[strategyIndex] = this.strategicAdmin;
      this.finalMenuItem[samplingIndex] = this.samplingAdminMenu;
    } else {
      if (currentPath.includes(this.stringConstants.strategicRoutePathName) && currentPath.includes('create') ) {
        this.router.navigate([this.stringConstants.unauthorizedPath]);
      }
      if (currentPath.includes(this.stringConstants.samplingRoutePathName)) {
        this.router.navigate([this.stringConstants.unauthorizedPath]);
      }
      this.finalMenuItem[strategyIndex] = this.strategicUser;
    }
  }
}
