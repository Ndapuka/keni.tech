import { Component, ElementRef, ViewChild, AfterViewInit, OnDestroy, inject, signal } from '@angular/core';
import { GsapAnimationService } from '../../../core/services/gsap-animation.service';
import { FaqItem } from '../../models/faq-item.models';

@Component({
  selector: 'app-faq',
  standalone: true,
  imports: [],
  templateUrl: './faq.html',
  styleUrl: './faq.scss',
})
export class Faq implements AfterViewInit, OnDestroy {
  @ViewChild('section', { static: true }) sectionRef!: ElementRef<HTMLElement>;

  private gsapService = inject(GsapAnimationService);
  private ctx?: gsap.Context;

  openId = signal<string | null>('q1');

  items: FaqItem[] = [
    {
      id: 'q1',
      question: 'How long does it take to set up KeNI?',
      answer: 'Most businesses are fully online within minutes using our guided wizard — no technical knowledge required.',
    },
    {
      id: 'q2',
      question: 'Do I need to buy new hardware?',
      answer: 'No. KeNI works with most existing POS terminals, printers and scanners. New hardware is optional.',
    },
    {
      id: 'q3',
      question: 'Can I change plans later?',
      answer: 'Yes, you can upgrade or downgrade your plan at any time from your dashboard, with no penalties.',
    },
    {
      id: 'q4',
      question: 'Does KeNI support multiple locations?',
      answer: 'Yes, from the Professional plan upward you can manage multiple locations from a single account.',
    },
    {
      id: 'q5',
      question: 'Is my data secure?',
      answer: 'All data is encrypted in transit and at rest, with isolated storage per business (multi-tenant architecture).',
    },
  ];

  toggle(id: string): void {
    this.openId.set(this.openId() === id ? null : id);
  }

  ngAfterViewInit(): void {
    this.ctx = this.gsapService.core.context(() => {
      const gsap = this.gsapService.core;
      const items = this.sectionRef.nativeElement.querySelectorAll('.faq-item');

      gsap.set(items, { opacity: 0, y: 24 });
      gsap.to(items, {
        opacity: 1,
        y: 0,
        duration: 0.6,
        ease: 'power3.out',
        stagger: 0.08,
        scrollTrigger: { trigger: this.sectionRef.nativeElement, start: 'top 80%', once: true },
      });
    }, this.sectionRef.nativeElement);
  }

  ngOnDestroy(): void {
    this.ctx?.revert();
  }
}