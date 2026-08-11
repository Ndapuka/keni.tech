import { Component, ElementRef, ViewChild, AfterViewInit, OnDestroy, inject } from '@angular/core';
import { GsapAnimationService } from '../../../core/services/gsap-animation.service';

@Component({
  selector: 'app-why-keni',
  standalone: true,
  imports: [],
  templateUrl: './why-keni.html',
  styleUrl: './why-keni.scss',
})
export class WhyKeni implements AfterViewInit, OnDestroy {
  @ViewChild('section', { static: true }) sectionRef!: ElementRef<HTMLElement>;

  private gsapService = inject(GsapAnimationService);
  private ctx?: gsap.Context;

  // TODO: substituir por copy final
  reasons = [
    {
      index: '01',
      title: 'One platform, not five subscriptions',
      description:
        'Orders, payments, inventory and reservations in a single system — no more juggling disconnected tools.',
    },
    {
      index: '02',
      title: 'Set up in minutes, not weeks',
      description:
        'Guided onboarding gets your business online and taking orders the same day you sign up.',
    },
    {
      index: '03',
      title: 'Built to grow with you',
      description:
        'Start with one location. Scale to ten. The platform scales with zero migration pain.',
    },
    {
      index: '04',
      title: 'Data that actually helps',
      description:
        'Real-time dashboards turn raw sales data into decisions you can act on today.',
    },
  ];

  ngAfterViewInit(): void {
    this.ctx = this.gsapService.core.context(() => {
      this.animateReveal();
      this.pinSticky();
    }, this.sectionRef.nativeElement);
  }

  private animateReveal(): void {
    const gsap = this.gsapService.core;
    const items = this.sectionRef.nativeElement.querySelectorAll('.why-item');

    gsap.set(items, { opacity: 0.25, y: 30 });

    items.forEach((item) => {
      gsap.to(item, {
        opacity: 1,
        y: 0,
        duration: 0.6,
        ease: 'power2.out',
        scrollTrigger: {
          trigger: item,
          start: 'top 75%',
          end: 'top 40%',
          scrub: true,
        },
      });
    });
  }

  private pinSticky(): void {
    const ScrollTrigger = this.gsapService.scrollTrigger;

    ScrollTrigger.create({
      trigger: this.sectionRef.nativeElement,
      start: 'top top',
      end: 'bottom bottom',
      pin: this.sectionRef.nativeElement.querySelector('.why-sticky'),
      pinSpacing: false,
    });
  }

  ngOnDestroy(): void {
    this.ctx?.revert();
  }
}