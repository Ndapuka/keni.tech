import { Component, input } from '@angular/core';

@Component({
    selector: 'app-insight-card',
    standalone: true,
    templateUrl: './insight-card.html',
    styleUrl: './insight-card.scss',
})
export class InsightCard {
    title = input.required<string>();
    description = input.required<string>();
    ctaLabel = input<string>();
}