// features/landing/pages/home/home.ts
import { Component, ViewChild, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AfterViewInit, ElementRef } from '@angular/core';

import { CommonModule } from '@angular/common';
import { gsap } from 'gsap';

import { Navbar } from '../../../../shared/components/navbar/navbar';
import { Hero } from '../../../auth/components/hero/hero';
import { AuthModal } from '../../../auth/components/auth-modal/auth-modal';
import { Products } from '../../../../shared/components/products/products';
import { Ai } from '../../../../shared/components/ai/ai';
import { DashboardShowcase } from '../../../../shared/components/dashboard-showcase/dashboard-showcase';
import { Faq } from '../../../../shared/components/faq/faq';
import { Footer } from '../../../../shared/components/footer/footer';
import { Hardware } from '../../../../shared/components/hardware/hardware';
import { CtaBanner } from '../../../../shared/components/cta-banner/cta-banner';
import { Testimonials } from '../../../../shared/components/testimonials/testimonials';
import { TrustedByBusiness } from '../../../../shared/components/trusted-by-business/trusted-by-business';
import { Pricing } from '../../../../shared/components/pricing/pricing';
import { Features } from '../../../../shared/components/features/features';
import { WhyKeni } from '../../../../shared/components/why-keni/why-keni';
import { DashboardPreview } from '../../../landing/components/dashboard-preview/dashboard-previes';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    Navbar,
    Hero,
    WhyKeni,
    Products,
    CtaBanner,
    Testimonials,
    Hardware,
    Features,
    Ai,
    DashboardShowcase,
    Pricing,
    Faq,
    Footer,
    TrustedByBusiness,
    AuthModal,
    DashboardPreview
  ],
  templateUrl: './home.html',
  styleUrls: ['./home.scss']
})
export class Home implements OnInit {

  private route = inject(ActivatedRoute);
  private router = inject(Router);

  @ViewChild(Hero)
  hero!: Hero;

  showAuthModal = false;
  ngOnInit(): void {
    this.route.queryParamMap.subscribe(params => {
      if (params.get('openAuth') === 'login') {
        this.showAuthModal = true;
        // limpa o query param para não reabrir o modal num refresh
        this.router.navigate([], { queryParams: {} });
      }
    });
  }

  openAuthModal(): void {
    console.log('openAuthModal called');
    this.showAuthModal = true;
  }

  closeAuthModal(): void {
    console.log('closeAuthModal called');
    this.showAuthModal = false;
  }

  onLoginSuccess(): void {
    this.closeAuthModal();
    // TODO: quando o CompanyService/guard estiverem prontos,
    // decidir aqui entre /dashboard e /company/create
  }

  onPreloaderFinished(): void {
    this.hero.startHeroAnimation();
  }
}

