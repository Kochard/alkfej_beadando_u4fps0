import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeasurementResults } from './measurement-results';

describe('MeasurementResults', () => {
  let component: MeasurementResults;
  let fixture: ComponentFixture<MeasurementResults>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MeasurementResults],
    }).compileComponents();

    fixture = TestBed.createComponent(MeasurementResults);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
