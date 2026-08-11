import { Component, inject } from '@angular/core';
import { ThemeService } from '../../../core/services/theme.service';

@Component({
    selector: 'app-theme-toggle',
    standalone: true,
    imports: [],
    templateUrl: './theme-toggle.html',
    styleUrl: './theme-toggle.scss',
})
export class ThemeToggle {
    themeService = inject(ThemeService);
}