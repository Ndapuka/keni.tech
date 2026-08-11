import {
  Component,
  ElementRef,
  ViewChild,
  AfterViewInit,
  OnDestroy,
  inject,
  signal,
} from '@angular/core';
import { GsapAnimationService } from '../../../core/services/gsap-animation.service';
import { ProductService } from '../../models/product-service.model';

interface ExtendedItem {
  service: ProductService;
  key: string;
  realIndex: number;
}

const CLONE_COUNT = 2;

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [],
  templateUrl: './products.html',
  styleUrl: './products.scss',
})
export class Products implements AfterViewInit, OnDestroy {
  @ViewChild('section', { static: true }) sectionRef!: ElementRef<HTMLElement>;
  @ViewChild('track', { static: true }) trackRef!: ElementRef<HTMLElement>;

  private gsapService = inject(GsapAnimationService);
  private ctx?: gsap.Context;
  private onWheel = (e: WheelEvent) => this.handleWheel(e);
  private onScroll = () => this.onScrollDebounced();
  private onResize = () => this.jumpToExtendedIndex(this.getNearestExtendedIndex(), false);
  private settleTimeout?: ReturnType<typeof setTimeout>;
  private isJumping = false;

  activeIndex = signal(0); // índice REAL (0..n-1) — usado pelos dots

  services: ProductService[] = [
    { id: 'orders', icon: '/images/placeholders/service-orders.svg', title: 'Orders', tagline: 'From table to kitchen in seconds.', features: ['Digital menu', 'Table management', 'Split billing'] },
    { id: 'payments', icon: '/images/placeholders/service-payments.svg', title: 'Payments', tagline: 'Accept every payment method, safely.', features: ['Card & contactless', 'QR payments', 'Instant receipts'] },
    { id: 'inventory', icon: '/images/placeholders/service-inventory.svg', title: 'Inventory', tagline: 'Never run out at the worst moment.', features: ['Stock tracking', 'Low-stock alerts', 'Supplier orders'] },
    { id: 'reservations', icon: '/images/placeholders/service-reservations.svg', title: 'Reservations', tagline: 'Fill every table, every night.', features: ['Online booking', 'Table planner', 'No-show protection'] },
    { id: 'analytics', icon: '/images/placeholders/service-analytics.svg', title: 'Analytics', tagline: 'Know what\u2019s working, in real time.', features: ['Revenue reports', 'Customer insights', 'Trend forecasts'] },
  ];

  // Lista estendida: clones do fim colados ao início, clones do início colados ao fim.
  // É isto que garante que existe sempre "vizinho" visível dos dois lados, mesmo nas pontas reais.
  extendedList: ExtendedItem[] = this.buildExtendedList();

  private buildExtendedList(): ExtendedItem[] {
    const n = this.services.length;
    const before = this.services.slice(n - CLONE_COUNT).map((s, i) => ({
      service: s, key: `pre-${i}`, realIndex: n - CLONE_COUNT + i,
    }));
    const main = this.services.map((s, i) => ({ service: s, key: `main-${i}`, realIndex: i }));
    const after = this.services.slice(0, CLONE_COUNT).map((s, i) => ({
      service: s, key: `post-${i}`, realIndex: i,
    }));
    return [...before, ...main, ...after];
  }

  ngAfterViewInit(): void {
    this.ctx = this.gsapService.core.context(() => {
      this.animateEntry();
    }, this.sectionRef.nativeElement);

    const track = this.trackRef.nativeElement;
    track.addEventListener('wheel', this.onWheel, { passive: false });
    track.addEventListener('scroll', this.onScroll, { passive: true });
    window.addEventListener('resize', this.onResize);

    // posição inicial: centra o 1º card real (índice CLONE_COUNT na lista estendida), sem animação
    requestAnimationFrame(() => this.jumpToExtendedIndex(CLONE_COUNT, false));
  }

  private animateEntry(): void {
    const gsap = this.gsapService.core;
    const cards = this.trackRef.nativeElement.querySelectorAll('.product-card');

    gsap.set(cards, { opacity: 0, y: 40 });
    gsap.to(cards, {
      opacity: 1,
      y: 0,
      duration: 0.7,
      ease: 'power3.out',
      stagger: 0.06,
      scrollTrigger: { trigger: this.sectionRef.nativeElement, start: 'top 75%', once: true },
    });
  }

  // ── Wheel com inércia suave: GSAP anima o scrollLeft em vez de o alterar de forma instantânea ─────────────────────────
  private handleWheel(e: WheelEvent): void {
    e.preventDefault();
    const gsap = this.gsapService.core;
    const el = this.trackRef.nativeElement;
    const delta = e.deltaY !== 0 ? e.deltaY : e.deltaX;
    const target = el.scrollLeft + delta * 1.6;

    gsap.to(el, {
      scrollLeft: target,
      duration: 0.7,
      ease: 'power3.out',
      overwrite: true,
      onUpdate: () => this.updateActiveIndexFromScroll(),
    });

    this.scheduleSettle();
  }

  private onScrollDebounced(): void {
    this.updateActiveIndexFromScroll();
    this.scheduleSettle();
  }

  // espera o scroll "assentar" (220ms sem novo evento) antes de verificar se estamos numa zona clonada
  private scheduleSettle(): void {
    clearTimeout(this.settleTimeout);
    this.settleTimeout = setTimeout(() => this.settleLoop(), 220);
  }

  private getCardStep(): number {
    const card = this.trackRef.nativeElement.querySelector('.product-card') as HTMLElement | null;
    const gap = 32;
    return (card?.offsetWidth ?? 0) + gap;
  }

  private getNearestExtendedIndex(): number {
    const el = this.trackRef.nativeElement;
    const step = this.getCardStep();
    if (step === 0) return CLONE_COUNT;
    return Math.round(el.scrollLeft / step);
  }

  private updateActiveIndexFromScroll(): void {
    if (this.isJumping) return;
    const index = this.getNearestExtendedIndex();
    const clamped = Math.max(0, Math.min(this.extendedList.length - 1, index));
    this.activeIndex.set(this.extendedList[clamped].realIndex);
  }

  // Se o scroll assentou dentro da zona de clones, salta silenciosamente (sem animação)
  // para a posição equivalente na zona real. Como o clone é visualmente idêntico,
  // o utilizador não perceciona o salto — é isto que cria o efeito de loop infinito.
  private settleLoop(): void {
    const n = this.services.length;
    const index = this.getNearestExtendedIndex();

    if (index < CLONE_COUNT) {
      this.jumpToExtendedIndex(index + n, false);
    } else if (index >= CLONE_COUNT + n) {
      this.jumpToExtendedIndex(index - n, false);
    }
  }

  private jumpToExtendedIndex(extendedIndex: number, animate: boolean): void {
    const el = this.trackRef.nativeElement;
    const step = this.getCardStep();
    const target = extendedIndex * step;

    if (!animate) {
      this.isJumping = true;
      el.scrollLeft = target;
      this.updateActiveIndexFromScroll();
      requestAnimationFrame(() => (this.isJumping = false));
      return;
    }

    this.gsapService.core.to(el, {
      scrollLeft: target,
      duration: 0.6,
      ease: 'power3.out',
      onUpdate: () => this.updateActiveIndexFromScroll(),
      onComplete: () => this.scheduleSettle(),
    });
  }

  // ── Navegação por setas/dots ─────────────────────────
  goNext(): void {
    this.jumpToExtendedIndex(this.getNearestExtendedIndex() + 1, true);
  }

  goPrev(): void {
    this.jumpToExtendedIndex(this.getNearestExtendedIndex() - 1, true);
  }

  scrollToRealIndex(realIndex: number): void {
    // navega sempre pelo caminho mais curto (para a frente OU para trás),
    // nunca "volta atrás" de forma contraintuitiva ao clicar num dot
    const current = this.getNearestExtendedIndex();
    const clamped = Math.max(0, Math.min(this.extendedList.length - 1, current));
    const currentReal = this.extendedList[clamped].realIndex;
    const n = this.services.length;

    let diff = realIndex - currentReal;
    if (diff > n / 2) diff -= n;
    if (diff < -n / 2) diff += n;

    this.jumpToExtendedIndex(current + diff, true);
  }

  ngOnDestroy(): void {
    clearTimeout(this.settleTimeout);
    const track = this.trackRef.nativeElement;
    track.removeEventListener('wheel', this.onWheel);
    track.removeEventListener('scroll', this.onScroll);
    window.removeEventListener('resize', this.onResize);
    this.ctx?.revert();
  }
}