import { ACMPresentationAC } from '../../swaggerapi/AngularFiles';
import { BehaviorSubject } from 'rxjs';

export interface AcmAddTabs  {
  id?: string;
  title?: string;
  content: string;
  disabled?: boolean;
  removable?: boolean;
  active: boolean;
  acmDetails: ACMPresentationAC;
  customClass?: string;
  temporaryFiles?: Array<File>;
}
