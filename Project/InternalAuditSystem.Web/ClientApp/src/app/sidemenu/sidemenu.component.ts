import { Component, ViewChild, OnInit, Input, ElementRef } from '@angular/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { NavItem } from '../sidebar-item';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sidemenu-items',
  templateUrl: './sidemenu.component.html',
  styleUrls: ['./sidemenu.component.scss'],
  animations: [
    // Added animation for rotate icons in sidebar menu
    trigger('indicatorRotate', [
      state('collapsed', style({ transform: 'rotate(0deg)' })),
      state('expanded', style({ transform: 'rotate(180deg)' })),
      transition('expanded <=> collapsed',
        animate('225ms cubic-bezier(0.4,0.0,0.2,1)')
      ),
    ])
  ]
})

export class SidemenuComponent implements OnInit {
  expanded: boolean;
  @Input() item: NavItem;
  @Input() depth: number;
  toggleSideBar: boolean;
  selectedItem = 0;
  @ViewChild('menu', { static: false }) menu: ElementRef;

  constructor(private elementRef: ElementRef, public router: Router) {

    // Condition that checks whether menu has a child and its depth is undefined or not.
    if (this.depth === undefined) {
      this.depth = 0;
    }
  }

  /*** Lifecycle hook that is called after data-bound properties of a       directive are initialized.
    *  Initialization of properties.
    */
  ngOnInit() {

  }

  // Method to display multilevel submenus
  onItemSelected(item: NavItem) {
    if (!item.children || !item.children.length) {
      this.router.navigate([item.route]);
    }
    if (item.children && item.children.length) {
      this.expanded = !this.expanded;
    }

  }
}
