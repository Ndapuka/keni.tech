export interface BusinessHealth {
    score: number; // 0-100
    status: 'critical' | 'warning' | 'good' | 'excellent';
    missingItems: string[];
}

export interface SalesTrendPoint {
    label: string;
    value: number;
}

export interface SalesTrend {
    currentValue: number;
    changePercent: number;
    currency: string;
    points: SalesTrendPoint[];
}

export interface LiveEvent {
    id: string;
    icon: 'order' | 'payment' | 'reservation' | 'review';
    message: string;
    timestamp: Date;
}

export interface Insight {
    id: string;
    title: string;
    description: string;
    ctaLabel?: string;
}
export interface DashboardView {
    id: string;
    label: string;
    screenshot: string; // placeholder 
    description: string;
}