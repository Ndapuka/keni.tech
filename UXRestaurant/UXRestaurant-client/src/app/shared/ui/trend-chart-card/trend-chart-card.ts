import { Component, ElementRef, ViewChild, input, computed, AfterViewInit, OnDestroy, inject } from '@angular/core';
import { GsapAnimationService } from '../../../core/services/gsap-animation.service';
import { SalesTrendPoint } from '../../models/metrics-dashboard-models';

@Component({
    selector: 'app-trend-chart-card',
    standalone: true,
    templateUrl: './trend-chart-card.html',
    styleUrl: './trend-chart-card.scss',
})
export class TrendChartCard implements AfterViewInit, OnDestroy {
    title = input('Sales Trend');
    currentValue = input.required<number>();
    currency = input('€');
    changePercent = input.required<number>();
    points = input.required<SalesTrendPoint[]>();

    @ViewChild('card', { static: true }) cardRef!: ElementRef<HTMLElement>;

    private gsapService = inject(GsapAnimationService);
    private ctx?: gsap.Context;

    isPositive = computed(() => this.changePercent() >= 0);

    pathD = computed(() => {
        const values = this.points().map((p) => p.value);
        const max = Math.max(...values);
        const min = Math.min(...values);
        const range = max - min || 1;
        const stepX = 300 / (values.length - 1);

        return values
            .map((v, i) => {
                const x = i * stepX;
                const y = 80 - ((v - min) / range) * 70;
                return `${i === 0 ? 'M' : 'L'}${x},${y}`;
            })
            .join(' ');
    });

    ngAfterViewInit(): void {
        this.ctx = this.gsapService.core.context(() => {
            const gsap = this.gsapService.core;
            const path = this.cardRef.nativeElement.querySelector('.trend-line') as SVGPathElement;
            const length = path.getTotalLength();

            gsap.set(path, { strokeDasharray: length, strokeDashoffset: length });

            gsap.to(path, {
                strokeDashoffset: 0,
                duration: 1.2,
                ease: 'power2.out',
                scrollTrigger: { trigger: this.cardRef.nativeElement, start: 'top 80%', once: true },
            });
        }, this.cardRef.nativeElement);
    }

    ngOnDestroy(): void {
        this.ctx?.revert();
    }
}