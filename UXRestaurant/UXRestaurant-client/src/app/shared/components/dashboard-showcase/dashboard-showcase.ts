import {
  Component,
  ElementRef,
  ViewChild,
  AfterViewInit,
  OnDestroy,
  inject,
  signal,
} from '@angular/core';
import { GsapAnimationService } from '../../../core/services/gsap-animation.service';
import { DashboardView } from '../../models/metrics-dashboard-models';

@Component({
  selector: 'app-dashboard-showcase',
  standalone: true,
  imports: [],
  templateUrl: './dashboard-showcase.html',
  styleUrl: './dashboard-showcase.scss',
})
export class DashboardShowcase implements AfterViewInit, OnDestroy {
  @ViewChild('section', { static: true }) sectionRef!: ElementRef<HTMLElement>;
  @ViewChild('pinTarget', { static: true }) pinTargetRef!: ElementRef<HTMLElement>;

  private gsapService = inject(GsapAnimationService);
  private ctx?: gsap.Context;

  activeIndex = signal(0);

  // TODO: substituir screenshots por imagens reais quando existirem
  views: DashboardView[] = [
    {
      id: 'analytics',
      label: 'Analytics',
      screenshot: '/images/placeholders/dashboard-analytics.png',
      description: 'Revenue, orders and customer trends at a glance.',
    },
    {
      id: 'inventory',
      label: 'Inventory',
      screenshot: '/images/placeholders/dashboard-inventory.png',
      description: 'Stock levels, alerts and automatic reorder points.',
    },
    {
      id: 'pos',
      label: 'POS',
      screenshot: '/images/placeholders/dashboard-pos.png',
      description: 'A fast, intuitive point of sale for every table.',
    },
    {
      id: 'orders',
      label: 'Orders',
      screenshot: '/images/placeholders/dashboard-orders.png',
      description: 'Track every order from kitchen to table in real time.',
    },
    {
      id: 'kitchen',
      label: 'Kitchen',
      screenshot: '/images/placeholders/dashboard-kitchen.png',
      description: 'A live kitchen display that keeps every station in sync.',
    },
    {
      id: 'reports',
      label: 'Reports',
      screenshot: '/images/placeholders/dashboard-reports.png',
      description: 'Exportable reports for revenue, staff and inventory.',
    },
  ];

  ngAfterViewInit(): void {
    this.ctx = this.gsapService.core.context(() => {
      const ScrollTrigger = this.gsapService.scrollTrigger;
      const totalViews = this.views.length;

      ScrollTrigger.create({
        trigger: this.sectionRef.nativeElement,
        start: 'top top',
        end: `+=${(totalViews - 1) * 100}%`,
        pin: this.pinTargetRef.nativeElement,
        scrub: 1,
        onUpdate: (self) => {
          const index = Math.min(
            totalViews - 1,
            Math.round(self.progress * (totalViews - 1))
          );
          if (index !== this.activeIndex()) {
            this.activeIndex.set(index);
          }
        },
      });
    }, this.sectionRef.nativeElement);
  }

  ngOnDestroy(): void {
    this.ctx?.revert();
  }
}