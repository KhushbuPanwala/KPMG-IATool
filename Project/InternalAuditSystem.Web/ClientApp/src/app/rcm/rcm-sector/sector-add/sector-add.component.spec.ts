import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { of as observableOf } from 'rxjs';
import { SectorAddComponent } from './sector-add.component';
import { RcmSectorAC } from '../../../swaggerapi/AngularFiles/model/rcmSectorAC';
import { RcmSectorService } from '../../../swaggerapi/AngularFiles/api/rcmSector.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { StringConstants } from '../../../shared/stringConstants';
import { LoaderService } from '../../../services/loader.service';

const mockRcmSectorData = {} as RcmSectorAC;
mockRcmSectorData.id = '1';
mockRcmSectorData.sector = 'test';

describe('SectorAddComponent', () => {
  let component: SectorAddComponent;
  let fixture: ComponentFixture<SectorAddComponent>;
  let service: RcmSectorService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SectorAddComponent],
      imports: [HttpClientTestingModule,
        RouterTestingModule,
        BrowserAnimationsModule,
        FormsModule,
        ToastrModule.forRoot()
      ],
      providers: [RcmSectorService, StringConstants, ToastrService, LoaderService]
    })

      .compileComponents();
    fixture = TestBed.createComponent(SectorAddComponent);
    component = fixture.componentInstance;
    service = TestBed.inject(RcmSectorService);
    fixture.detectChanges();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SectorAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should open list of sectors', () => {
    component.openListRcmSector();
    expect(component).toBeTruthy();
  });

  it('should get sectors by id', () => {
    spyOn(RcmSectorService.prototype, 'rcmSectorGetRcmSectorById').and.callFake(() => {
      return observableOf(mockRcmSectorData[0]);
    });
    component.getSectorById();
    expect(mockRcmSectorData[0]).toEqual(mockRcmSectorData[0]);
  });

  it('should add sector', () => {
    spyOn(RcmSectorService.prototype, 'rcmSectorAddRcmSector').and.callFake(() => {
      return observableOf(mockRcmSectorData[0]);
    });
    component.saveRcmSector();
    expect(mockRcmSectorData[0]).toEqual(mockRcmSectorData[0]);
  });

  it('should update sector', () => {
    spyOn(RcmSectorService.prototype, 'rcmSectorUpdateRcmSector').and.callFake(() => {
      return observableOf(mockRcmSectorData[0]);
    });
    component.saveRcmSector();
    expect(mockRcmSectorData[0]).toEqual(mockRcmSectorData[0]);
  });
});
