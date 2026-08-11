export interface BusinessType {
    slug: string;
    name: string;
    icon: string;
    description: string;
}

export const BUSINESS_TYPES: BusinessType[] = [
    { slug: 'restaurant', name: 'Restaurant', icon: '/images/placeholders/biz-restaurant.svg', description: 'Full-service dining, orders and tables.' },
    { slug: 'cafe-bakery', name: 'Café & Bakery', icon: '/images/placeholders/biz-cafe.svg', description: 'Fast counter service and daily specials.' },
    { slug: 'retail', name: 'Retail', icon: '/images/placeholders/biz-retail.svg', description: 'Inventory-driven, multi-location ready.' },
    { slug: 'barber-shop', name: 'Barber Shop', icon: '/images/placeholders/biz-barber.svg', description: 'Appointments, staff schedules and walk-ins.' },
    { slug: 'spa', name: 'Spa', icon: '/images/placeholders/biz-spa.svg', description: 'Bookings, packages and client history.' },
    { slug: 'bar-nightclub', name: 'Bar & Nightclub', icon: '/images/placeholders/biz-bar.svg', description: 'Fast tabs, table service, late-night speed.' },
];