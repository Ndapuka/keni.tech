import { Component, signal, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ThemeService } from './core/services/theme.service';
//import { ThemeToggle } from './shared/ui/theme-toggle/theme-toggle';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet
    //ThemeToggle


  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('UXRestaurant-client');
}
