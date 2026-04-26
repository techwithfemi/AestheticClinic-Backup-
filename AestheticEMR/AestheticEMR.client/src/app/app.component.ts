import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToastaModule, ToastaConfig, ToastaService, ToastOptions } from 'ngx-toasta';
import { Subscription } from 'rxjs';
import { AuthService } from './services/auth.service';
import { AppTranslationService } from './services/app-translation.service';
import { AlertService, AlertCommand, MessageSeverity } from './services/alert.service';

@Component({
    selector: 'app-root',
    template: `
      <ngx-toasta></ngx-toasta>
      <router-outlet></router-outlet>
    `,
    standalone: true,
    imports: [ToastaModule, RouterOutlet]
})
export class AppComponent implements OnInit, OnDestroy {
  private readonly toastaConfig = inject(ToastaConfig);
  private readonly toastaService = inject(ToastaService);
  private readonly alertService = inject(AlertService);
  private readonly authService = inject(AuthService);
  private readonly translationService = inject(AppTranslationService);
  private messageSubscription?: Subscription;

  constructor() {
    // Keep global toast configurations here
    this.toastaConfig.theme = 'bootstrap';
    this.toastaConfig.position = 'top-right';
    this.toastaConfig.limit = 5;
  }

  ngOnInit() {
    // Initialize global language settings
    this.translationService.getTranslation('app.Notifications');

    // Reactivate global toast notifications
    this.messageSubscription = this.alertService.getMessageEvent()
      .subscribe(command => this.handleAlertCommand(command));

    // Check if user session is still valid on refresh
    if (this.authService.isLoggedIn) {
      console.log('User session restored');
    }
  }

  ngOnDestroy(): void {
    this.messageSubscription?.unsubscribe();
  }

  private handleAlertCommand(command: AlertCommand): void {
    if (command.operation === 'clear') {
      this.toastaService.clearAll();
      return;
    }

    if (!command.message) {
      return;
    }

    const options: ToastOptions = {
      title: command.message.summary,
      msg: command.message.detail,
      showClose: true,
      timeout: command.operation === 'add_sticky' ? 0 : this.toastaConfig.timeout,
      onRemove: command.onRemove
    };

    switch (command.message.severity) {
      case MessageSeverity.success:
        this.toastaService.success(options);
        break;
      case MessageSeverity.error:
        this.toastaService.error(options);
        break;
      case MessageSeverity.warn:
        this.toastaService.warning(options);
        break;
      case MessageSeverity.wait:
        this.toastaService.wait(options);
        break;
      case MessageSeverity.info:
        this.toastaService.info(options);
        break;
      default:
        this.toastaService.default(options);
        break;
    }
  }
}
