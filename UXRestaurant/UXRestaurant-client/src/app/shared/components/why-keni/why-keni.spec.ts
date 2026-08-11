import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WhyKeni } from './why-keni';

describe('WhyKeni', () => {
  let component: WhyKeni;
  let fixture: ComponentFixture<WhyKeni>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WhyKeni],
    }).compileComponents();

    fixture = TestBed.createComponent(WhyKeni);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
