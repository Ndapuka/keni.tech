import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  Output,
  ViewChild
} from '@angular/core';

import { gsap } from 'gsap';

@Component({
  selector: 'app-preloader',
  standalone: true,
  templateUrl: './preloader.html',
  styleUrls: ['./preloader.scss']
})
export class Preloader implements AfterViewInit {

  @Output()
  finished = new EventEmitter<void>();

  @ViewChild('loaderContainer')
  loaderContainer!: ElementRef<HTMLDivElement>;

  @ViewChild('imagesContainer')
  imagesContainer!: ElementRef<HTMLDivElement>;

  @ViewChild('blocTxt')
  blocTxt!: ElementRef<HTMLDivElement>;

  @ViewChild('title')
  title!: ElementRef<HTMLHeadingElement>;

  @ViewChild('flipOverlay')
  flipOverlay!: ElementRef<HTMLDivElement>;

  @ViewChild('flipImg1')
  flipImg1!: ElementRef<HTMLDivElement>;

  ngAfterViewInit(): void {

    this.startLoaderAnimation();

  }

  private startLoaderAnimation(): void {

    const timeline = gsap.timeline({

      defaults: {

        ease: 'power2'

      }

    });

    timeline

      .to(this.imagesContainer.nativeElement, {

        height: 400,

        duration: 1.3,

        delay: .4

      })

      .to(this.blocTxt.nativeElement, {

        height: 'auto',

        duration: .6

      }, '-=.8')

      .to(this.title.nativeElement, {

        y: 0

      }, '-=.6')

      .to(this.flipOverlay.nativeElement, {

        y: 0,

        duration: .6

      })

      .set(this.flipImg1.nativeElement, {

        display: 'none'

      })

      .to(this.flipOverlay.nativeElement, {

        y: '-100%'

      })

      .to(this.loaderContainer.nativeElement, {

        autoAlpha: 0,

        duration: .8,

        delay: .7

      })

      .add(() => {

        this.loaderContainer.nativeElement.style.display = 'none';

        this.finished.emit();

      });

  }

}