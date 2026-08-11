import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { BusinessHealth, SalesTrend, LiveEvent, Insight } from '../../../shared/models/metrics-dashboard-models';

@Injectable({ providedIn: 'root' })
export class DashboardPreviewService {
    // TODO: substituir por chamadas reais ao Company/Order Service via ApiService
    getBusinessHealth(): Observable<BusinessHealth> {
        return of({
            score: 87,
            status: 'good',
            missingItems: ['No QR Menu', 'No delivery settings'],
        });
    }

    getSalesTrend(): Observable<SalesTrend> {
        return of({
            currentValue: 12450,
            changePercent: 18.4,
            currency: '€',
            points: [
                { label: 'Mon', value: 800 }, { label: 'Tue', value: 950 },
                { label: 'Wed', value: 780 }, { label: 'Thu', value: 1200 },
                { label: 'Fri', value: 1450 }, { label: 'Sat', value: 1900 },
                { label: 'Sun', value: 1620 },
            ],
        });
    }

    getLiveEvents(): Observable<LiveEvent[]> {
        return of([
            { id: '1', icon: 'order', message: 'New order #4821 — €38.50', timestamp: new Date() },
            { id: '2', icon: 'payment', message: 'Payment confirmed — Table 6', timestamp: new Date() },
            { id: '3', icon: 'reservation', message: 'New reservation for tonight, 8PM', timestamp: new Date() },
            { id: '4', icon: 'review', message: 'New 5★ review received', timestamp: new Date() },
            { id: '5', icon: 'order', message: 'New order #4820 — €22.00', timestamp: new Date() },
        ]);
    }

    getInsights(): Observable<Insight> {
        return of({
            id: '1',
            title: 'Your Friday sales are peaking earlier',
            description: 'Consider opening 30 minutes earlier on Fridays — orders are trending up before your current opening time.',
            ctaLabel: 'View full report',
        });
    }
}