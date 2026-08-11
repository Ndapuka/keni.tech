import { Component, ElementRef, ViewChild, AfterViewInit, OnDestroy, inject } from '@angular/core';
import { GsapAnimationService } from '../../../core/services/gsap-animation.service';

@Component({
  selector: 'app-cta-banner',
  standalone: true,
  imports: [],
  templateUrl: './cta-banner.html',
  styleUrl: './cta-banner.scss',
})
export class CtaBanner implements AfterViewInit, OnDestroy {
  @ViewChild('section', { static: true }) sectionRef!: ElementRef<HTMLElement>;

  private gsapService = inject(GsapAnimationService);
  private ctx?: gsap.Context;

  ngAfterViewInit(): void {
    this.ctx = this.gsapService.core.context(() => {
      const gsap = this.gsapService.core;
      const els = this.sectionRef.nativeElement.querySelectorAll('.cta-animate');

      gsap.set(els, { opacity: 0, y: 30 });
      gsap.to(els, {
        opacity: 1,
        y: 0,
        duration: 0.8,
        ease: 'power3.out',
        stagger: 0.12,
        scrollTrigger: { trigger: this.sectionRef.nativeElement, start: 'top 75%', once: true },
      });
    }, this.sectionRef.nativeElement);
  }

  ngOnDestroy(): void {
    this.ctx?.revert();
  }
}
