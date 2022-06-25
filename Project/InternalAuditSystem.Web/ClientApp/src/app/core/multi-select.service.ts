import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay, map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

// Interface for rcm properties
export interface Rcm {
  id: string;
  name: string;
  disabled?: boolean;
}

@Injectable({
  providedIn: 'root'
})

export class MultiSelectService {
  constructor(private http: HttpClient) { }
  // Method to get rcm item
  getRcm(term: string = null): Observable<Rcm[]> {
    let items = getMockRcm();
    if (term) {
      items = items.filter(x => x.name.toLocaleLowerCase().indexOf(term.toLocaleLowerCase()) > -1);
    }
    return of(items).pipe(delay(500));
  }
}

// Function to get rcm name
function getMockRcm() {
  return [
    {
      id: '5a15b13c36e7a7f00cf0d7cb',
      name: 'Risk Description1',
    },
    {
      id: '5a15b13c36e7a7f00cf0d7cb',
      name: 'Risk Description2',
    },
    {
      id: '5a15b13c36e7a7f00cf0d7cb',
      name: 'Risk Description3',
    },
  ];
}
