import {
    AfterViewInit,
    Component,
    ElementRef,
    ViewChild
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { gsap } from 'gsap';

@Component({
    selector: 'app-hero',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './hero.html',
    styleUrls: ['./hero.scss']
})
export class Hero implements AfterViewInit {

    @ViewChild('videoPlayer')
    videoPlayer!: ElementRef<HTMLVideoElement>;

    @ViewChild('overlay')
    overlay!: ElementRef<HTMLDivElement>;

    @ViewChild('homeContent')
    homeContent!: ElementRef<HTMLDivElement>;

    startHeroAnimation(): void {

        this.videoPlayer.nativeElement.play();

        gsap.timeline()

            .to(this.videoPlayer.nativeElement, {

                opacity: 1,

                duration: .8

            })

            .to(this.overlay.nativeElement, {

                opacity: 1,

                duration: .6

            }, "-=.6")

            .to(this.homeContent.nativeElement, {

                opacity: 1,

                y: 0,

                duration: 1

            }, "-=.4");

    }
    ngAfterViewInit(): void {

        const video = this.videoPlayer.nativeElement;

        video.muted = true;

        video.play().catch(error => {

            console.log(error);

        });

    }
}