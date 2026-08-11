import { Component, ElementRef, ViewChild, AfterViewInit, OnDestroy, inject, signal } from '@angular/core';
import { GsapAnimationService } from '../../../core/services/gsap-animation.service';
import { Testimonial, TrustMetric } from '../../models/testimonial.model';

@Component({
  selector: 'app-testimonials',
  standalone: true,
  imports: [],
  templateUrl: './testimonials.html',
  styleUrl: './testimonials.scss',
})
export class Testimonials implements AfterViewInit, OnDestroy {
  @ViewChild('section', { static: true }) sectionRef!: ElementRef<HTMLElement>;
  @ViewChild('track', { static: true }) trackRef!: ElementRef<HTMLElement>;

  private gsapService = inject(GsapAnimationService);
  private ctx?: gsap.Context;
  private autoplayTween?: gsap.core.Tween;

  // Enquanto não há clientes reais, esta flag decide o que mostrar.
  // Trocar para `true` assim que existirem testemunhos reais.
  hasRealTestimonials = signal(false);

  trustMetrics: TrustMetric[] = [
    { id: 'm1', value: '6', label: 'Independent microservices' },
    { id: 'm2', value: '99.9%', label: 'Uptime target' },
    { id: 'm3', value: '< 5 min', label: 'Average setup time' },
    { id: 'm4', value: '24/7', label: 'AI-guided support' },
  ];

  // TODO: substituir por testemunhos reais quando existirem clientes
  testimonials: Testimonial[] = [
    {
      id: 't1',
      quote: 'Setting up took minutes, not weeks. The AI assistant walked us through everything.',
      authorName: 'Placeholder Name',
      authorRole: 'Owner',
      businessName: 'Placeholder Restaurant',
      avatar: '/images/placeholders/avatar-1.png',
    },
    {
      id: 't2',
      quote: 'Having orders, payments and inventory in one place changed how we run the floor.',
      authorName: 'Placeholder Name',
      authorRole: 'Manager',
      businessName: 'Placeholder Café',
      avatar: '/images/placeholders/avatar-2.png',
    },
    {
      id: 't3',
      quote: 'The forecasting alone paid for the subscription in the first month.',
      authorName: 'Placeholder Name',
      authorRole: 'Owner',
      businessName: 'Placeholder Bakery',
      avatar: '/images/placeholders/avatar-3.png',
    },
  ];

  ngAfterViewInit(): void {
    this.ctx = this.gsapService.core.context(() => {
      this.animateEntry();
      if (this.hasRealTestimonials()) {
        this.initAutoplay();
      }
    }, this.sectionRef.nativeElement);
  }

  private animateEntry(): void {
    const gsap = this.gsapService.core;
    const els = this.sectionRef.nativeElement.querySelectorAll('.entry-animate');

    gsap.set(els, { opacity: 0, y: 30 });
    gsap.to(els, {
      opacity: 1,
      y: 0,
      duration: 0.7,
      ease: 'power3.out',
      stagger: 0.1,
      scrollTrigger: { trigger: this.sectionRef.nativeElement, start: 'top 78%', once: true },
    });
  }

  private initAutoplay(): void {
    const gsap = this.gsapService.core;
    const track = this.trackRef.nativeElement;
    const distance = track.scrollWidth - track.clientWidth;

    if (distance <= 0) return;

    this.autoplayTween = gsap.to(track, {
      scrollLeft: distance,
      duration: 24,
      ease: 'none',
      repeat: -1,
      yoyo: true,
    });

    track.addEventListener('mouseenter', () => this.autoplayTween?.pause());
    track.addEventListener('mouseleave', () => this.autoplayTween?.resume());
  }

  ngOnDestroy(): void {
    this.autoplayTween?.kill();
    this.ctx?.revert();
  }
}