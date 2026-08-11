import {
  Component,
  ElementRef,
  ViewChild,
  AfterViewInit,
  OnDestroy,
  inject,
} from '@angular/core';
import { GsapAnimationService } from '../../../core/services/gsap-animation.service';
import { HardwareDevice, CompatiblePartner } from '../../models/hardware-device';

@Component({
  selector: 'app-hardware',
  standalone: true,
  imports: [],
  templateUrl: './hardware.html',
  styleUrl: './hardware.scss',
})
export class Hardware implements AfterViewInit, OnDestroy {
  @ViewChild('section', { static: true }) sectionRef!: ElementRef<HTMLElement>;
  @ViewChild('grid', { static: true }) gridRef!: ElementRef<HTMLElement>;

  private gsapService = inject(GsapAnimationService);
  private ctx?: gsap.Context;

  devices: HardwareDevice[] = [
    { id: 'pda', icon: '/images/placeholders/hw-pda.svg', name: 'PDA', description: 'Take orders tableside, fully wireless.' },
    { id: 'pos-terminal', icon: '/images/placeholders/hw-pos.svg', name: 'POS Terminal', description: 'A fast, reliable checkout counter.' },
    { id: 'kitchen-display', icon: '/images/placeholders/hw-kds.svg', name: 'Kitchen Display', description: 'Orders sync instantly to the kitchen.' },
    { id: 'printer', icon: '/images/placeholders/hw-printer.svg', name: 'Receipt Printer', description: 'Fast, silent thermal printing.' },
    { id: 'scanner', icon: '/images/placeholders/hw-scanner.svg', name: 'Barcode Scanner', description: 'Scan products and stock in seconds.' },
    { id: 'cash-drawer', icon: '/images/placeholders/hw-cash.svg', name: 'Cash Drawer', description: 'Secure, auto-opens on checkout.' },
    { id: 'customer-display', icon: '/images/placeholders/hw-display.svg', name: 'Customer Display', description: 'Transparent pricing at every sale.' },
    { id: 'mobile-pos', icon: '/images/placeholders/hw-mobile.svg', name: 'Mobile POS', description: 'Turn any tablet into a full register.' },
  ];

  partners: CompatiblePartner[] = [
    { id: 'p1', name: 'Sunmi', logo: '/images/placeholders/partner-sunmi.svg' },
    { id: 'p2', name: 'Epson', logo: '/images/placeholders/partner-epson.svg' },
    { id: 'p3', name: 'Star Micronics', logo: '/images/placeholders/partner-star.svg' },
    { id: 'p4', name: 'Zebra', logo: '/images/placeholders/partner-zebra.svg' },
    { id: 'p5', name: 'PAX', logo: '/images/placeholders/partner-pax.svg' },
  ];

  ngAfterViewInit(): void {
    this.ctx = this.gsapService.core.context(() => {
      this.animateGridReveal();
      this.initTiltEffect();
    }, this.sectionRef.nativeElement);
  }

  private animateGridReveal(): void {
    const gsap = this.gsapService.core;
    const cards = this.gridRef.nativeElement.querySelectorAll('.hardware-card');

    gsap.set(cards, { opacity: 0, y: 50, rotateX: -15, transformPerspective: 800 });

    gsap.to(cards, {
      opacity: 1,
      y: 0,
      rotateX: 0,
      duration: 0.8,
      ease: 'power3.out',
      stagger: { each: 0.08, grid: 'auto', from: 'start' },
      scrollTrigger: {
        trigger: this.gridRef.nativeElement,
        start: 'top 80%',
        once: true,
      },
    });
  }

  // efeito de "luz" a seguir o cursor + leve tilt 3D — evoca a "iluminação" descrita no plano original
  private initTiltEffect(): void {
    const cards = this.gridRef.nativeElement.querySelectorAll<HTMLElement>('.hardware-card');

    cards.forEach((card) => {
      const onMove = (e: MouseEvent) => {
        const rect = card.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const y = e.clientY - rect.top;
        const rotateX = ((y / rect.height) - 0.5) * -10;
        const rotateY = ((x / rect.width) - 0.5) * 10;

        card.style.setProperty('--spot-x', `${x}px`);
        card.style.setProperty('--spot-y', `${y}px`);
        card.style.transform = `perspective(800px) rotateX(${rotateX}deg) rotateY(${rotateY}deg) scale(1.03)`;
      };

      const onLeave = () => {
        card.style.transform = 'perspective(800px) rotateX(0) rotateY(0) scale(1)';
      };

      card.addEventListener('mousemove', onMove);
      card.addEventListener('mouseleave', onLeave);
    });
  }

  ngOnDestroy(): void {
    this.ctx?.revert();
  }
}
