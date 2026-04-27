import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToastaModule, ToastaConfig, ToastaService, ToastOptions } from 'ngx-toasta';
import { Subscription } from 'rxjs';
import { AuthService } from './services/auth.service';
import { AppTranslationService } from './services/app-translation.service';
import { AlertService, AlertCommand, AlertDialog, DialogType, MessageSeverity } from './services/alert.service';

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
  private dialogSubscription?: Subscription;

  constructor() {
    // Keep global toast configurations here
    this.toastaConfig.theme = 'bootstrap';
    this.toastaConfig.position = 'top-right';
    this.toastaConfig.limit = 5;
    this.toastaConfig.timeout = 5000;
  }

  ngOnInit() {
    // Initialize global language settings
    this.translationService.getTranslation('app.Notifications');

    // Reactivate global toast notifications
    this.messageSubscription = this.alertService.getMessageEvent()
      .subscribe(command => this.handleAlertCommand(command));

    // Reactivate global dialog handling (confirm/prompt/alert)
    this.dialogSubscription = this.alertService.getDialogEvent()
      .subscribe(dialog => this.handleAlertDialog(dialog));

    // Check if user session is still valid on refresh
    if (this.authService.isLoggedIn) {
      console.log('User session restored');
    }
  }

  ngOnDestroy(): void {
    this.messageSubscription?.unsubscribe();
    this.dialogSubscription?.unsubscribe();
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
      timeout: 5000,
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

  private handleAlertDialog(dialog: AlertDialog): void {
    const message = this.normalizeDialogMessage(dialog.message);

    switch (dialog.type) {
      case DialogType.confirm: {
        const confirmed = window.confirm(message);
        if (confirmed) {
          dialog.okCallback?.();
        } else {
          dialog.cancelCallback?.();
        }
        break;
      }
      case DialogType.prompt: {
        const value = window.prompt(message, dialog.defaultValue ?? '');
        if (value !== null) {
          dialog.okCallback?.(value);
        } else {
          dialog.cancelCallback?.();
        }
        break;
      }
      default:
        window.alert(message);
        dialog.okCallback?.();
        break;
    }
  }

  private normalizeDialogMessage(message: string): string {
    return message
      .replace(/<br\s*\/?>/gi, '\n')
      .replace(/<[^>]*>/g, '')
      .trim();
  }
}
