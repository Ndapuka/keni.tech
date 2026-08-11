export interface ProductMenuItem {
    slug: string;
    name: string;
    icon: string;
    description: string;
}

export const PRODUCTS_MENU: ProductMenuItem[] = [
    { slug: 'orders', name: 'Orders', icon: '/images/placeholders/nav-orders.svg', description: 'From table to kitchen in seconds.' },
    { slug: 'payments', name: 'Payments', icon: '/images/placeholders/nav-payments.svg', description: 'Every payment method, safely.' },
    { slug: 'inventory', name: 'Inventory', icon: '/images/placeholders/nav-inventory.svg', description: 'Stock tracking and low-stock alerts.' },
    { slug: 'reservations', name: 'Reservations', icon: '/images/placeholders/nav-reservations.svg', description: 'Table booking that fills every seat.' },
    { slug: 'analytics', name: 'Analytics', icon: '/images/placeholders/nav-analytics.svg', description: 'Revenue and customer insights.' },
    { slug: 'hardware', name: 'Hardware', icon: '/images/placeholders/nav-hardware.svg', description: 'PDAs, terminals and printers.' },
    { slug: 'ai-assistant', name: 'AI Assistant', icon: '/images/placeholders/nav-ai.svg', description: 'Guided setup and daily suggestions.' },
];