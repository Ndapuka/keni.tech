import { Injectable, signal } from '@angular/core';

export type Theme = 'dark' | 'light';

const STORAGE_KEY = 'keni-theme';

@Injectable({ providedIn: 'root' })
export class ThemeService {
    // O valor inicial é lido do que o script anti-FOUC já aplicou no <html>,
    // não recalculado aqui — evita os dois lados divergirem.
    readonly theme = signal<Theme>(this.readCurrentTheme());

    constructor() {
        // sincroniza entre abas: se o utilizador mudar o tema noutra aba, esta aba acompanha
        window.addEventListener('storage', (event) => {
            if (event.key === STORAGE_KEY && event.newValue) {
                this.applyTheme(event.newValue as Theme, false);
            }
        });
    }

    toggle(): void {
        const next: Theme = this.theme() === 'dark' ? 'light' : 'dark';
        this.applyTheme(next, true);
    }

    setTheme(theme: Theme): void {
        this.applyTheme(theme, true);
    }

    private applyTheme(theme: Theme, persist: boolean): void {
        document.documentElement.setAttribute('data-theme', theme);
        this.theme.set(theme);

        if (persist) {
            localStorage.setItem(STORAGE_KEY, theme);
        }
    }

    private readCurrentTheme(): Theme {
        const attr = document.documentElement.getAttribute('data-theme');
        return attr === 'light' ? 'light' : 'dark';
    }
}