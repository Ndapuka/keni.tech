import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardShowcase } from './dashboard-showcase';

describe('DashboardShowcase', () => {
  let component: DashboardShowcase;
  let fixture: ComponentFixture<DashboardShowcase>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardShowcase],
    }).compileComponents();

    fixture = TestBed.createComponent(DashboardShowcase);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
