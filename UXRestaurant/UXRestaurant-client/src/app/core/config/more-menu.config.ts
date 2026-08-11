export interface MoreMenuLink {
    label: string;
    href: string;
    description: string;
}

export const MORE_MENU: MoreMenuLink[] = [
    { label: 'About', href: '#', description: 'Our mission and story.' },
    { label: 'Help Center', href: '#', description: 'Guides and documentation.' },
    { label: 'Careers', href: '#', description: 'Join the team.' },
    { label: 'Contact', href: '#', description: 'Talk to sales or support.' },
    { label: 'FAQ', href: '#faq', description: 'Common questions, answered.' },
];