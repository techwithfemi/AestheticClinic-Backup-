import { Component, OnInit, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToastaModule, ToastaConfig } from 'ngx-toasta';
import { AuthService } from './services/auth.service';
import { AppTranslationService } from './services/app-translation.service';

@Component({
    selector: 'app-root',
    template: `
      <ngx-toasta></ngx-toasta>
      <router-outlet></router-outlet>
    `,
    standalone: true,
    imports: [ToastaModule, RouterOutlet]
})
export class AppComponent implements OnInit {
  private readonly toastaConfig = inject(ToastaConfig);
  private readonly authService = inject(AuthService);
  private readonly translationService = inject(AppTranslationService);

  constructor() {
    // Keep global toast configurations here
    this.toastaConfig.theme = 'bootstrap';
    this.toastaConfig.position = 'top-right';
    this.toastaConfig.limit = 5;
  }

  ngOnInit() {
    // Initialize global language settings
    this.translationService.getTranslation('app.Notifications');

    // Check if user session is still valid on refresh
    if (this.authService.isLoggedIn) {
      console.log('User session restored');
    }
  }
}
