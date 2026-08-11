import { Component, ElementRef, ViewChild, AfterViewInit, OnDestroy, inject } from '@angular/core';
import { GsapAnimationService } from '../../../core/services/gsap-animation.service';
import { FeatureItem } from '../../models/feature-item.model';

@Component({
  selector: 'app-features',
  standalone: true,
  imports: [],
  templateUrl: './features.html',
  styleUrl: './features.scss',
})
export class Features implements AfterViewInit, OnDestroy {
  @ViewChild('section', { static: true }) sectionRef!: ElementRef<HTMLElement>;
  @ViewChild('grid', { static: true }) gridRef!: ElementRef<HTMLElement>;

  private gsapService = inject(GsapAnimationService);
  private ctx?: gsap.Context;

  features: FeatureItem[] = [
    {
      id: 'realtime-sync',
      icon: '/images/placeholders/feat-realtime.svg',
      title: 'Real-Time Sync',
      description: 'Every order, payment and stock change reflects instantly across every device and location.',
      size: 'lg',
    },
    { id: 'cloud-based', icon: '/images/placeholders/feat-cloud.svg', title: 'Cloud Based', description: 'Access your business from anywhere, on any device.', size: 'sm' },
    { id: 'offline', icon: '/images/placeholders/feat-offline.svg', title: 'Works Offline', description: 'Keep selling even without internet — syncs automatically once back online.', size: 'sm' },
    {
      id: 'ai-automation',
      icon: '/images/placeholders/feat-ai-auto.svg',
      title: 'AI Automation',
      description: 'Repetitive tasks, forecasts and restocking handled automatically — so you focus on the business, not the busywork.',
      size: 'lg',
    },
    { id: 'multi-location', icon: '/images/placeholders/feat-multiloc.svg', title: 'Multi-Location', description: 'Manage every location from a single account.', size: 'sm' },
    { id: 'role-permissions', icon: '/images/placeholders/feat-roles.svg', title: 'Role Permissions', description: 'Control exactly what each team member can see and do.', size: 'sm' },
    { id: 'secure-payments', icon: '/images/placeholders/feat-secpay.svg', title: 'Secure Payments', description: 'PCI-compliant payment processing, built in.', size: 'sm' },
    { id: 'open-api', icon: '/images/placeholders/feat-api.svg', title: 'Open API', description: 'Integrate KeNI with the tools you already use.', size: 'sm' },
    { id: 'business-insights', icon: '/images/placeholders/feat-insights.svg', title: 'Business Insights', description: 'Data that turns into decisions, automatically.', size: 'sm' },
    { id: 'fast-setup', icon: '/images/placeholders/feat-setup.svg', title: 'Fast Setup', description: 'Go live in minutes, not weeks.', size: 'sm' },
    { id: 'backups', icon: '/images/placeholders/feat-backup.svg', title: 'Automatic Backups', description: 'Your data, always safe, always recoverable.', size: 'sm' },
    {
      id: 'enterprise-security',
      icon: '/images/placeholders/feat-security.svg',
      title: 'Enterprise Security',
      description: 'End-to-end encryption and isolated multi-tenant architecture, protecting every business on the platform.',
      size: 'lg',
    },
  ];

  ngAfterViewInit(): void {
    this.ctx = this.gsapService.core.context(() => {
      this.animateGridReveal();
    }, this.sectionRef.nativeElement);
  }

  private animateGridReveal(): void {
    const gsap = this.gsapService.core;
    const cards = this.gridRef.nativeElement.querySelectorAll('.feature-card');

    gsap.set(cards, { opacity: 0, y: 30, scale: 0.96 });
    gsap.to(cards, {
      opacity: 1,
      y: 0,
      scale: 1,
      duration: 0.6,
      ease: 'power3.out',
      stagger: { each: 0.06, grid: 'auto', from: 'start' },
      scrollTrigger: { trigger: this.gridRef.nativeElement, start: 'top 82%', once: true },
    });
  }

  ngOnDestroy(): void {
    this.ctx?.revert();
  }
}