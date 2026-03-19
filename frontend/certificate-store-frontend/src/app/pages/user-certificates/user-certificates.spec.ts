import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserCertificates } from './user-certificates';

describe('UserCertificates', () => {
  let component: UserCertificates;
  let fixture: ComponentFixture<UserCertificates>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserCertificates],
    }).compileComponents();

    fixture = TestBed.createComponent(UserCertificates);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
