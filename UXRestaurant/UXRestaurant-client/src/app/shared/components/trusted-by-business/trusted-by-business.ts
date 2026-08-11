import {
  Component,
  ElementRef,
  ViewChild,
  AfterViewInit,
  OnDestroy,
  inject,
} from '@angular/core';
import { GsapAnimationService } from '../../../core/services/gsap-animation.service';

@Component({
  selector: 'app-trusted-by-business',
  standalone: true,
  imports: [],
  templateUrl: './trusted-by-business.html',
  styleUrl: './trusted-by-business.scss',
})
export class TrustedByBusiness implements AfterViewInit, OnDestroy {
  @ViewChild('section', { static: true }) sectionRef!: ElementRef<HTMLElement>;
  @ViewChild('scrollTrack', { static: true }) scrollTrackRef!: ElementRef<HTMLElement>;
  @ViewChild('marquee', { static: true }) marqueeRef!: ElementRef<HTMLElement>;

  private gsapService = inject(GsapAnimationService);
  private ctx?: gsap.Context;
  private onWheel = (e: WheelEvent) => this.handleWheel(e);

  businesses = [
    { name: 'Restaurant', icon: '/images/placeholders/restaurant.svg' },
    { name: 'Café', icon: '/images/placeholders/cafe.svg' },
    { name: 'Retail', icon: '/images/placeholders/retail.svg' },
    { name: 'Barber Shop', icon: '/images/placeholders/barber.svg' },
    { name: 'Spa', icon: '/images/placeholders/spa.svg' },
    { name: 'Bakery', icon: '/images/placeholders/bakery.svg' },
  ];

  ngAfterViewInit(): void {
    this.ctx = this.gsapService.core.context(() => {
      this.animateCardsReveal();
      this.animateMarquee();
    }, this.sectionRef.nativeElement);

    this.scrollTrackRef.nativeElement.addEventListener('wheel', this.onWheel, {
      passive: false,
    });
  }

  private animateCardsReveal(): void {
    const gsap = this.gsapService.core;
    const cards = this.scrollTrackRef.nativeElement.querySelectorAll('.business-card');

    gsap.set(cards, { opacity: 0, y: 30 });

    gsap.to(cards, {
      opacity: 1,
      y: 0,
      duration: 0.7,
      ease: 'power3.out',
      stagger: 0.1,
      scrollTrigger: {
        trigger: this.scrollTrackRef.nativeElement,
        start: 'top 82%',
        once: true,
      },
    });
  }

  private animateMarquee(): void {
    const gsap = this.gsapService.core;
    const track = this.marqueeRef.nativeElement.querySelector('.marquee-track') as HTMLElement;
    if (!track) return;

    track.innerHTML += track.innerHTML;
    const totalWidth = track.scrollWidth / 2;

    gsap.to(track, {
      x: -totalWidth,
      duration: 20,
      ease: 'none',
      repeat: -1,
    });
  }

  private handleWheel(e: WheelEvent): void {
    const el = this.scrollTrackRef.nativeElement;
    const atStart = el.scrollLeft <= 0 && e.deltaY < 0;
    const atEnd =
      el.scrollLeft + el.clientWidth >= el.scrollWidth - 1 && e.deltaY > 0;

    // deixa a página continuar a fazer scroll vertical normal
    // quando o carrossel já chegou ao fim de qualquer um dos lados
    if (atStart || atEnd) return;

    e.preventDefault();
    el.scrollLeft += e.deltaY;
  }

  ngOnDestroy(): void {
    this.scrollTrackRef.nativeElement.removeEventListener('wheel', this.onWheel);
    this.ctx?.revert();
  }
}