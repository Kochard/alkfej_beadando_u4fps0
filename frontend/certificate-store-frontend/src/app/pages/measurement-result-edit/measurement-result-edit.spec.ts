import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeasurementResultEdit } from './measurement-result-edit';

describe('MeasurementResultEdit', () => {
  let component: MeasurementResultEdit;
  let fixture: ComponentFixture<MeasurementResultEdit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MeasurementResultEdit],
    }).compileComponents();

    fixture = TestBed.createComponent(MeasurementResultEdit);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
