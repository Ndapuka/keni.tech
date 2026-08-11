import { Component, ElementRef, ViewChild, AfterViewInit, OnDestroy, inject } from '@angular/core';
import { GsapAnimationService } from '../../../core/services/gsap-animation.service';
import { PricingPlan } from '../../models/pricing-plan.model';

@Component({
  selector: 'app-pricing',
  standalone: true,
  imports: [],
  templateUrl: './pricing.html',
  styleUrl: './pricing.scss',
})
export class Pricing implements AfterViewInit, OnDestroy {
  @ViewChild('section', { static: true }) sectionRef!: ElementRef<HTMLElement>;

  private gsapService = inject(GsapAnimationService);
  private ctx?: gsap.Context;

  plans: PricingPlan[] = [
    {
      id: 'starter',
      name: 'Starter',
      price: 29,
      period: '/month',
      description: 'For single-location businesses just getting started.',
      features: ['1 location', 'Orders & Payments', 'Basic reports', 'Email support'],
      ctaLabel: 'Start Free',
      highlighted: false,
    },
    {
      id: 'professional',
      name: 'Professional',
      price: 79,
      period: '/month',
      description: 'For growing businesses that need the full toolkit.',
      features: [
        'Up to 5 locations',
        'Orders, Payments & Inventory',
        'Reservations',
        'AI Insights & Forecasts',
        'Priority support',
      ],
      ctaLabel: 'Get Professional',
      highlighted: true,
    },
    {
      id: 'enterprise',
      name: 'Enterprise',
      price: 0,
      period: 'Custom',
      description: 'For multi-location and franchise operations.',
      features: [
        'Unlimited locations',
        'Dedicated account manager',
        'Custom integrations',
        'SLA & onboarding support',
      ],
      ctaLabel: 'Contact Sales',
      highlighted: false,
    },
  ];

  ngAfterViewInit(): void {
    this.ctx = this.gsapService.core.context(() => {
      const gsap = this.gsapService.core;
      const cards = this.sectionRef.nativeElement.querySelectorAll('.pricing-card');

      gsap.set(cards, { opacity: 0, y: 40 });
      gsap.to(cards, {
        opacity: 1,
        y: 0,
        duration: 0.7,
        ease: 'power3.out',
        stagger: 0.12,
        scrollTrigger: { trigger: this.sectionRef.nativeElement, start: 'top 78%', once: true },
      });
    }, this.sectionRef.nativeElement);
  }

  ngOnDestroy(): void {
    this.ctx?.revert();
  }
}
