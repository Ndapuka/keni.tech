import { Component, ElementRef, ViewChild, AfterViewInit, OnDestroy, inject, signal } from '@angular/core';
import { KpiHealthCard } from '../../../../shared/ui/kpi-health-card/kpi-health-card';
import { TrendChartCard } from '../../../../shared/ui/trend-chart-card/trend-chart-card';
import { LiveFeedCard } from '../../../../shared/ui/live-feed-card/live-feed-card';
import { InsightCard } from '../../../../shared/ui/insight-card/insight-card';
import { DashboardPreviewService } from '../../services/dashboard-preview.service';
import { GsapAnimationService } from '../../../../core/services/gsap-animation.service';
import { BusinessHealth, SalesTrend, LiveEvent, Insight } from '../../../../shared/models/metrics-dashboard-models';

@Component({
    selector: 'app-dashboard-preview',
    standalone: true,
    imports: [KpiHealthCard, TrendChartCard, LiveFeedCard, InsightCard],
    templateUrl: './dashboard-preview.html',
    styleUrl: './dashboard-preview.scss',
})
export class DashboardPreview implements AfterViewInit, OnDestroy {
    @ViewChild('section', { static: true }) sectionRef!: ElementRef<HTMLElement>;

    private dashboardService = inject(DashboardPreviewService);
    private gsapService = inject(GsapAnimationService);
    private ctx?: gsap.Context;

    health = signal<BusinessHealth | null>(null);
    trend = signal<SalesTrend | null>(null);
    events = signal<LiveEvent[]>([]);
    insight = signal<Insight | null>(null);

    constructor() {
        this.dashboardService.getBusinessHealth().subscribe((v) => this.health.set(v));
        this.dashboardService.getSalesTrend().subscribe((v) => this.trend.set(v));
        this.dashboardService.getLiveEvents().subscribe((v) => this.events.set(v));
        this.dashboardService.getInsights().subscribe((v) => this.insight.set(v));
    }

    ngAfterViewInit(): void {
        this.ctx = this.gsapService.core.context(() => {
            const gsap = this.gsapService.core;
            const cards = this.sectionRef.nativeElement.querySelectorAll('.bento-item');

            gsap.set(cards, { opacity: 0, y: 40 });
            gsap.to(cards, {
                opacity: 1,
                y: 0,
                duration: 0.7,
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