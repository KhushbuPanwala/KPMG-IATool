import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {

  public isLoading = new BehaviorSubject(false);
  constructor() { }

  /**
   * Open a loader
   */
  open() {
    this.isLoading.next(true);
  }

  /**
   * Closes a loader
   */
  close() {
    this.isLoading.next(false);
  }
}
