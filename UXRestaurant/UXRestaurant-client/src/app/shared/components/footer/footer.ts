import { Component } from '@angular/core';

interface FooterColumn {
  title: string;
  links: { label: string; href: string }[];
}

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [],
  templateUrl: './footer.html',
  styleUrl: './footer.scss',
})
export class Footer {
  columns: FooterColumn[] = [
    {
      title: 'Product',
      links: [
        { label: 'Features', href: '#' },
        { label: 'Orders', href: '#' },
        { label: 'Payments', href: '#' },
        { label: 'Inventory', href: '#' },
        { label: 'Reservations', href: '#' },
        { label: 'Analytics', href: '#' },
      ],
    },
    {
      title: 'Business Types',
      links: [
        { label: 'Restaurants', href: '#' },
        { label: 'Cafés & Bakeries', href: '#' },
        { label: 'Retail', href: '#' },
        { label: 'Barber Shops', href: '#' },
        { label: 'Spas', href: '#' },
      ],
    },
    {
      title: 'Company',
      links: [
        { label: 'About', href: '#' },
        { label: 'FAQ', href: '#' },
        { label: 'Partners', href: '#' },
        { label: 'Careers', href: '#' },
        { label: 'Contact', href: '#' },
      ],
    },
    {
      title: 'Legal',
      links: [
        { label: 'Privacy Policy', href: '#' },
        { label: 'Terms of Service', href: '#' },
        { label: 'Security', href: '#' },
      ],
    },
  ];

  currentYear = new Date().getFullYear();
}
