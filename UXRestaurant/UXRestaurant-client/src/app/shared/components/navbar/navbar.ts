// shared/components/navbar/navbar.ts
import { Component, EventEmitter, HostListener, Output, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { AuthFacadeService } from '../../../core/auth/services/auth-facade.service';
import { BUSINESS_TYPES } from '../../../core/config/business-types.config';
import { PRODUCTS_MENU } from '../../../core/config/products-menu.config';
import { PARTNERS } from '../../../core/config/partners.config';
import { MORE_MENU } from '../../../core/config/more-menu.config';
import { ThemeToggle } from '../../ui/theme-toggle/theme-toggle';

type MenuId = 'business' | 'products' | 'partners' | 'more' | null;

@Component({
    selector: 'app-navbar',
    standalone: true,
    imports: [CommonModule, ThemeToggle],
    templateUrl: './navbar.html',
    styleUrl: './navbar.scss',
})
export class Navbar {

    private authFacade = inject(AuthFacadeService);
    private router = inject(Router);

    @Output() loginClicked = new EventEmitter<void>();

    // signal direto do facade — sem ngOnInit, sem subscribe manual
    readonly user = this.authFacade.user;

    menuOpen = false;
    isScrolled = false;

    activeMenu = signal<MenuId>(null);
    private closeTimeout?: ReturnType<typeof setTimeout>;

    isMobileMenuOpen = signal(false);
    mobileAccordion = signal<MenuId>(null);

    readonly businessTypes = BUSINESS_TYPES;
    readonly productsMenu = PRODUCTS_MENU;
    readonly partners = PARTNERS;
    readonly moreMenu = MORE_MENU;

    @HostListener('window:scroll')
    onWindowScroll(): void {
        this.isScrolled = window.scrollY > 40;
    }

    openMenu(id: Exclude<MenuId, null>): void {
        clearTimeout(this.closeTimeout);
        this.activeMenu.set(id);
    }

    scheduleClose(): void {
        this.closeTimeout = setTimeout(() => this.activeMenu.set(null), 180);
    }

    cancelClose(): void {
        clearTimeout(this.closeTimeout);
    }

    toggleMobileMenu(): void {
        this.isMobileMenuOpen.update((v) => !v);
        document.body.style.overflow = this.isMobileMenuOpen() ? 'hidden' : '';

        if (!this.isMobileMenuOpen()) {
            this.mobileAccordion.set(null);
        }
    }

    toggleMobileAccordion(id: Exclude<MenuId, null>): void {
        this.mobileAccordion.update((current) => (current === id ? null : id));
    }

    closeMobileMenu(): void {
        this.isMobileMenuOpen.set(false);
        this.mobileAccordion.set(null);
        document.body.style.overflow = '';
    }

    openLogin(): void {
        this.closeMobileMenu();
        this.loginClicked.emit();
    }

    openProfile(): void {
        this.menuOpen = false;
        this.closeMobileMenu();
        this.router.navigate(['/profile']);
    }

    toggleMenu(): void {
        this.menuOpen = !this.menuOpen;
    }

    logout(): void {
        this.authFacade.logout();
        this.menuOpen = false;
    }
}