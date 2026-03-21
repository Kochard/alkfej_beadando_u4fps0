import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeasurementResultDetails } from './measurement-result-details';

describe('MeasurementResultDetails', () => {
  let component: MeasurementResultDetails;
  let fixture: ComponentFixture<MeasurementResultDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MeasurementResultDetails],
    }).compileComponents();

    fixture = TestBed.createComponent(MeasurementResultDetails);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
