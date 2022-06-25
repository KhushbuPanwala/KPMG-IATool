import { Component, OnInit } from '@angular/core';
import { LoaderService } from '../../core/loader.service';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.scss']
})
export class LoaderComponent implements OnInit {

  loading: boolean;

  constructor(private loaderService: LoaderService) {
    this.loaderService.isLoading.subscribe((v) => {
      this.loading = v;
    });
  }


  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
    *  Initialization of properties.
    */
  ngOnInit() {
  }

}
