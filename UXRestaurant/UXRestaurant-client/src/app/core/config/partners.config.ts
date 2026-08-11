export interface Partner {
    name: string;
    logo: string;
    category: 'software' | 'hardware' | 'coming-soon';
}

export const PARTNERS: Partner[] = [
    { name: 'Cegid', logo: '/images/partners/cegid.svg', category: 'software' },
    { name: 'Primavera', logo: '/images/partners/primavera.svg', category: 'software' },
    { name: 'Coming Soon', logo: '', category: 'coming-soon' },
    { name: 'Coming Soon', logo: '', category: 'coming-soon' },
];