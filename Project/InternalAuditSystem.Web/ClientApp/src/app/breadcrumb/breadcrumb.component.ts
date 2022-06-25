import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, NavigationEnd, PRIMARY_OUTLET } from '@angular/router';
import { filter } from 'rxjs/operators';
import { StringConstants } from '../shared/stringConstants';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html'
})

export class BreadcrumbComponent implements OnInit {

  breadCrumData: string[];

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
    *  Initialization of properties.
    */
  ngOnInit() {
    this.router.events.pipe(filter(event => event instanceof NavigationEnd)).subscribe(event => {
      // set breadcrumbs
      const root: ActivatedRoute = this.route.root;
      this.breadCrumData = this.getBreadcrumbs(root);

    });

  }
  constructor(private router: Router, private route: ActivatedRoute, private stringConstant: StringConstants) { }
  /**
   * To get Breadcrumbs data
   * @param route:active route
   */
  private getBreadcrumbs(route: ActivatedRoute): string[] {
    const ROUTE_DATA_BREADCRUMB = this.stringConstant.breadCrumbText;

    // get the child routes
    const children: ActivatedRoute[] = route.children;

    // iterate over each children
    for (const child of children) {
      // verify primary route
      if (child.outlet !== PRIMARY_OUTLET || child.snapshot.url.length === 0) {
        continue;
      }

      const breadcrumb = child.snapshot.data[ROUTE_DATA_BREADCRUMB];
      if (breadcrumb !== undefined) {
        this.breadCrumData = breadcrumb.split('>');
      }
      return this.breadCrumData;
    }
  }
}


