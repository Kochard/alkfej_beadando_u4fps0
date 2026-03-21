import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeasurementResultCreate } from './measurement-result-create';

describe('MeasurementResultCreate', () => {
  let component: MeasurementResultCreate;
  let fixture: ComponentFixture<MeasurementResultCreate>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MeasurementResultCreate],
    }).compileComponents();

    fixture = TestBed.createComponent(MeasurementResultCreate);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
