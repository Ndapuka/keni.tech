import { Component, ElementRef, ViewChild, AfterViewInit, OnDestroy, inject } from '@angular/core';
import { GsapAnimationService } from '../../../core/services/gsap-animation.service';
import { AiCapability } from '../../models/ai-capability.model';

@Component({
  selector: 'app-ai',
  standalone: true,
  imports: [],
  templateUrl: './ai.html',
  styleUrl: './ai.scss',
})
export class Ai implements AfterViewInit, OnDestroy {
  @ViewChild('section', { static: true }) sectionRef!: ElementRef<HTMLElement>;

  private gsapService = inject(GsapAnimationService);
  private ctx?: gsap.Context;

  capabilities: AiCapability[] = [
    {
      id: 'forecast',
      icon: '/images/placeholders/ai-forecast.svg',
      title: 'Forecast Sales',
      description: 'Predict next week\u2019s revenue based on real historical patterns, not guesswork.',
    },
    {
      id: 'inventory',
      icon: '/images/placeholders/ai-inventory.svg',
      title: 'Predict Inventory',
      description: 'Know exactly what to restock before you run out — automatically calculated.',
    },
    {
      id: 'insights',
      icon: '/images/placeholders/ai-insights.svg',
      title: 'Business Insights',
      description: 'Surface the patterns hiding in your data — peak hours, best-sellers, slow days.',
    },
    {
      id: 'assistant',
      icon: '/images/placeholders/ai-assistant.svg',
      title: 'AI Assistant',
      description: 'A guide that configures your restaurant and answers questions, step by step.',
    },
    {
      id: 'recommendations',
      icon: '/images/placeholders/ai-recommendations.svg',
      title: 'Smart Recommendations',
      description: 'Personalized suggestions for pricing, promotions and menu changes that work.',
    },
  ];

  ngAfterViewInit(): void {
    this.ctx = this.gsapService.core.context(() => {
      const gsap = this.gsapService.core;

      const visual = this.sectionRef.nativeElement.querySelector('.ai-visual');
      const items = this.sectionRef.nativeElement.querySelectorAll('.ai-capability');

      gsap.set(visual, { opacity: 0, scale: 0.92 });
      gsap.to(visual, {
        opacity: 1,
        scale: 1,
        duration: 1,
        ease: 'power3.out',
        scrollTrigger: { trigger: this.sectionRef.nativeElement, start: 'top 70%', once: true },
      });

      gsap.set(items, { opacity: 0, x: 30 });
      gsap.to(items, {
        opacity: 1,
        x: 0,
        duration: 0.6,
        ease: 'power3.out',
        stagger: 0.12,
        scrollTrigger: { trigger: this.sectionRef.nativeElement, start: 'top 65%', once: true },
      });
    }, this.sectionRef.nativeElement);
  }

  ngOnDestroy(): void {
    this.ctx?.revert();
  }
}