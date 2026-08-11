import { Injectable } from '@angular/core';
import gsap from 'gsap';
import { ScrollTrigger } from 'gsap/ScrollTrigger';

@Injectable({ providedIn: 'root' })
export class GsapAnimationService {
    constructor() {
        gsap.registerPlugin(ScrollTrigger);
    }

    get core() {
        return gsap;
    }

    get scrollTrigger() {
        return ScrollTrigger;
    }
}