export interface PricingPlan {
    id: string;
    name: string;
    price: number;
    period: string;
    description: string;
    features: string[];
    ctaLabel: string;
    highlighted: boolean;
}