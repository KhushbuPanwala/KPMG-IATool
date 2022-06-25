import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-unauthorized',
  templateUrl: 'unauthorized.component.html'
})

export class UnAuthorizedComponent {
  constructor(private router: Router) {
  }

  /**
   * Back to main site
   */
  backToMainSite() {
    this.router.navigate(['']);
  }
}
