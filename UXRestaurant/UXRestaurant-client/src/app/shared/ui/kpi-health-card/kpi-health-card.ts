import { Component, ElementRef, ViewChild, input, AfterViewInit, OnDestroy, inject, computed } from '@angular/core';
import { GsapAnimationService } from '../../../core/services/gsap-animation.service';

@Component({
    selector: 'app-kpi-health-card',
    standalone: true,
    templateUrl: './kpi-health-card.html',
    styleUrl: './kpi-health-card.scss',
})
export class KpiHealthCard implements AfterViewInit, OnDestroy {
    score = input.required<number>();
    status = input<'critical' | 'warning' | 'good' | 'excellent'>('good');
    missingItems = input<string[]>([]);

    @ViewChild('card', { static: true }) cardRef!: ElementRef<HTMLElement>;
    @ViewChild('counter', { static: true }) counterRef!: ElementRef<HTMLElement>;

    private gsapService = inject(GsapAnimationService);
    private ctx?: gsap.Context;

    readonly circumference = 2 * Math.PI * 54; // raio 54

    statusColor = computed(() => {
        const styles = getComputedStyle(document.documentElement);
        switch (this.status()) {
            case 'critical': return styles.getPropertyValue('--color-danger').trim();
            case 'warning': return styles.getPropertyValue('--color-warning').trim();
            case 'excellent': return styles.getPropertyValue('--color-accent').trim();
            default: return styles.getPropertyValue('--color-info').trim();
        }
    });

    ngAfterViewInit(): void {
        this.ctx = this.gsapService.core.context(() => {
            const gsap = this.gsapService.core;
            const target = { value: 0 };
            const circle = this.cardRef.nativeElement.querySelector('.gauge-progress') as SVGCircleElement;
            const offset = this.circumference * (1 - this.score() / 100);

            gsap.set(circle, { strokeDasharray: this.circumference, strokeDashoffset: this.circumference });

            gsap.to(target, {
                value: this.score(),
                duration: 1.4,
                ease: 'power2.out',
                onUpdate: () => {
                    this.counterRef.nativeElement.textContent = Math.round(target.value).toString();
                },
                scrollTrigger: { trigger: this.cardRef.nativeElement, start: 'top 80%', once: true },
            });

            gsap.to(circle, {
                strokeDashoffset: offset,
                duration: 1.4,
                ease: 'power2.out',
                scrollTrigger: { trigger: this.cardRef.nativeElement, start: 'top 80%', once: true },
            });
        }, this.cardRef.nativeElement);
    }

    ngOnDestroy(): void {
        this.ctx?.revert();
    }
}