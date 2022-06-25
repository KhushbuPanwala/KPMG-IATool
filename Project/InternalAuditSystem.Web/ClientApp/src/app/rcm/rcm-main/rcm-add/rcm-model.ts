import { RiskControlMatrixAC } from '../../../swaggerapi/AngularFiles';

export interface RcmTabs {
    id?: string;
    title?: string;
    content: string;
    disabled?: boolean;
    removable?: boolean;
    active: boolean;
    rcmDetails: RiskControlMatrixAC;
    customClass?: string;
  }
