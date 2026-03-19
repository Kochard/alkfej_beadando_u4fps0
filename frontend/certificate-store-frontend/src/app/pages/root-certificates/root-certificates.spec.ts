import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RootCertificates } from './root-certificates';

describe('RootCertificates', () => {
  let component: RootCertificates;
  let fixture: ComponentFixture<RootCertificates>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RootCertificates],
    }).compileComponents();

    fixture = TestBed.createComponent(RootCertificates);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
