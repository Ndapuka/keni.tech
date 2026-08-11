import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrustedByBusiness } from './trusted-by-business';

describe('TrustedByBusiness', () => {
  let component: TrustedByBusiness;
  let fixture: ComponentFixture<TrustedByBusiness>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrustedByBusiness],
    }).compileComponents();

    fixture = TestBed.createComponent(TrustedByBusiness);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
