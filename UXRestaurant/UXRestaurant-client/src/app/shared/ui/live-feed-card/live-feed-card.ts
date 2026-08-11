import { Component, input, DestroyRef, inject, signal, OnInit } from '@angular/core';
import { interval } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { LiveEvent } from '../../models/metrics-dashboard-models';

const ICONS: Record<LiveEvent['icon'], string> = {
    order: '🛎️',
    payment: '💳',
    reservation: '📅',
    review: '⭐',
};

@Component({
    selector: 'app-live-feed-card',
    standalone: true,
    imports: [],
    templateUrl: './live-feed-card.html',
    styleUrl: './live-feed-card.scss',
})
export class LiveFeedCard implements OnInit {
    initialEvents = input.required<LiveEvent[]>();
    intervalMs = input(3200);

    private destroyRef = inject(DestroyRef);
    visibleEvents = signal<LiveEvent[]>([]);
    icons = ICONS;

    ngOnInit(): void {
        console.log('LiveFeedCard ngOnInit chamado', this.initialEvents());
        this.visibleEvents.set(this.initialEvents().slice(0, 5));
        /*this.visibleEvents.set(this.initialEvents().slice(0, 5));

        let pointer = 0;
        interval(this.intervalMs())
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe(() => {
                const pool = this.initialEvents();
                const next = { ...pool[pointer % pool.length], id: crypto.randomUUID(), timestamp: new Date() };
                pointer++;

                this.visibleEvents.update((list) => [next, ...list].slice(0, 5));
            });
    }*/
    }
}