// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

import { Component, OnInit, OnDestroy, Input, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { AlertService, MessageSeverity, DialogType } from '../../services/alert.service';
import { AuthService } from '../../services/auth.service';
import { ConfigurationService } from '../../services/configuration.service';
import { Utilities } from '../../services/utilities';
import { UserLogin } from '../../models/user-login.model';
import { AccountService } from '../../services/account.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrl: './login.component.scss',
    imports: [
      FormsModule,
      TranslateModule,
      MatCardModule,
      MatFormFieldModule,
      MatInputModule,
      MatCheckboxModule,
      MatButtonModule,
      MatIconModule,
      MatProgressSpinnerModule
    ]
})

export class LoginComponent implements OnInit, OnDestroy {
  private alertService = inject(AlertService);
  private authService = inject(AuthService);
  private configurations = inject(ConfigurationService);
  private accountService = inject(AccountService);
  private route = inject(ActivatedRoute);

  userLogin = new UserLogin();
  isLoading = false;
  formResetToggle = true;
  modalClosedCallback: (() => void) | undefined;
  loginStatusSubscription: Subscription | undefined;

  forgotPasswordUserNameOrEmail = '';
  resetPasswordUserNameOrEmail = '';
  resetPasswordToken = '';
  resetPassword = '';
  resetPasswordConfirmation = '';
  isForgotPasswordMode = false;
  isResetPasswordMode = false;
  hidePassword = true;
  hideResetPassword = true;
  hideResetPasswordConfirmation = true;

  @Input()
  isModal = false;

  ngOnInit() {
    this.userLogin.rememberMe = this.authService.rememberMe;
    this.initializePasswordRecoveryMode();

    if (this.getShouldRedirect()) {
      this.authService.redirectLoginUser();
    } else {
      this.loginStatusSubscription = this.authService.getLoginStatusEvent().subscribe(() => {
        if (this.getShouldRedirect()) {
          this.authService.redirectLoginUser();
        }
      });
    }
  }

  ngOnDestroy() {
    this.loginStatusSubscription?.unsubscribe();
  }

  getShouldRedirect() {
    return !this.isModal && this.authService.isLoggedIn && !this.authService.isSessionExpired;
  }

  showErrorAlert(caption: string, message: string) {
    this.alertService.showMessage(caption, message, MessageSeverity.error);
  }

  closeModal() {
    if (this.modalClosedCallback) {
      this.modalClosedCallback();
    }
  }

  login() {
    this.isLoading = true;
    this.alertService.startLoadingMessage('', 'Attempting login...');

    this.authService.loginWithPassword(this.userLogin.userName, this.userLogin.password, this.userLogin.rememberMe)
      .subscribe({
        next: user => {
          setTimeout(() => {
            this.alertService.stopLoadingMessage();
            this.isLoading = false;
            this.reset();

            if (!this.isModal) {
              this.alertService.showMessage('Login', `Welcome ${user.userName}!`, MessageSeverity.success);
            } else {
              this.alertService.showMessage('Login', `Session for ${user.userName} restored!`, MessageSeverity.success);
              setTimeout(() => {
                this.alertService.showStickyMessage('Session Restored', 'Please try your last operation again', MessageSeverity.default);
              }, 500);

              this.closeModal();
            }
          }, 500);
        },
        error: error => {
          this.alertService.stopLoadingMessage();

          if (Utilities.checkNoNetwork(error)) {
            this.alertService.showStickyMessage(Utilities.noNetworkMessageCaption, Utilities.noNetworkMessageDetail, MessageSeverity.error, error);
            this.offerBackendDevServer();
          } else {
            const errorMessage = Utilities.getHttpResponseMessage(error);

            if (errorMessage) {
              this.alertService.showStickyMessage('Unable to login', this.mapLoginErrorMessage(errorMessage), MessageSeverity.error, error);
            } else {
              this.alertService.showStickyMessage('Unable to login',
                'An error occurred whilst logging in, please try again later.\nError: ' + Utilities.stringify(error), MessageSeverity.error, error);
            }
          }

          setTimeout(() => {
            this.isLoading = false;
          }, 500);
        }
      });
  }

  sendResetPasswordLink() {
    if (!this.forgotPasswordUserNameOrEmail?.trim()) {
      this.showErrorAlert('Username/email is required', 'Please enter a valid username or email address');
      return;
    }

    this.isLoading = true;
    this.alertService.startLoadingMessage('', 'Generating password reset mail...');

    const resetUrl = `${window.location.origin}/login?reset=true&userNameOrEmail={userNameOrEmail}&token={token}`;

    this.accountService.forgotPassword(this.forgotPasswordUserNameOrEmail.trim(), resetUrl)
      .subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.isLoading = false;
          this.alertService.showMessage('Recover Password', 'If the account exists, a password reset email has been sent.', MessageSeverity.success);
          this.cancelPasswordRecovery();
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.isLoading = false;
          this.alertService.showStickyMessage('Password Recovery Failed',
            'An error occurred whilst recovering your password.\nError: ' + Utilities.getHttpResponseMessage(error), MessageSeverity.error, error);
        }
      });
  }

  submitResetPassword() {
    if (!this.resetPasswordUserNameOrEmail?.trim()) {
      this.showErrorAlert('Username/email is required', 'Please enter a valid username or email address');
      return;
    }

    if (!this.resetPasswordToken?.trim()) {
      this.showErrorAlert('Reset token is required', 'Please use the reset link from your email.');
      return;
    }

    if (!this.resetPassword || this.resetPassword.length < 6) {
      this.showErrorAlert('New password is required', 'Please enter the new password (minimum of 6 characters)');
      return;
    }

    if (this.resetPassword !== this.resetPasswordConfirmation) {
      this.showErrorAlert('Password mismatch', 'New password and confirmation password do not match');
      return;
    }

    this.isLoading = true;
    this.alertService.startLoadingMessage('', 'Resetting password...');

    this.accountService.resetPassword(this.resetPasswordUserNameOrEmail.trim(), this.resetPasswordToken, this.resetPassword)
      .subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.isLoading = false;
          this.alertService.showMessage('Password Change', 'Your password was successfully reset. Please login.', MessageSeverity.success);
          this.cancelPasswordRecovery();
          this.userLogin.userName = this.resetPasswordUserNameOrEmail.trim();
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.isLoading = false;
          this.alertService.showStickyMessage('Password Reset Failed',
            'An error occurred whilst resetting your password.\nError: ' + Utilities.getHttpResponseMessage(error), MessageSeverity.error, error);
        }
      });
  }

  openForgotPassword() {
    this.isForgotPasswordMode = true;
    this.isResetPasswordMode = false;
  }

  cancelPasswordRecovery() {
    this.isForgotPasswordMode = false;
    this.isResetPasswordMode = false;
    this.forgotPasswordUserNameOrEmail = '';
    this.resetPassword = '';
    this.resetPasswordConfirmation = '';
    this.resetPasswordToken = '';
  }

  private initializePasswordRecoveryMode() {
    const queryMap = this.route.snapshot.queryParamMap;
    const shouldReset = queryMap.get('reset')?.toLowerCase() === 'true';

    if (!shouldReset) {
      return;
    }

    this.isResetPasswordMode = true;
    this.isForgotPasswordMode = false;
    this.resetPasswordUserNameOrEmail = queryMap.get('userNameOrEmail') ?? '';
    this.resetPasswordToken = queryMap.get('token') ?? '';
  }

  offerBackendDevServer() {
    if (Utilities.checkIsLocalHost(location.origin) && Utilities.checkIsLocalHost(this.configurations.baseUrl)) {
      if (!this.configurations.fallbackBaseUrl) {
        return;
      }
      this.alertService.showDialog(
        'Dear Developer!<br />' +
        'It appears your backend Web API server is inaccessible or not running...<br />' +
        'Would you want to temporarily switch to the fallback development API server below? (Or specify another)', DialogType.prompt, value => {
          this.configurations.baseUrl = value as string;
          this.alertService.showStickyMessage('API Changed!', 'The target Web API has been changed to: ' + value, MessageSeverity.warn);
        },
        null,
        null,
        null,
        this.configurations.fallbackBaseUrl);
    }
  }

  mapLoginErrorMessage(error: string) {
    if (error === 'invalid_username_or_password') {
      return 'Invalid username or password';
    }

    return error;
  }

  reset() {
    this.formResetToggle = false;

    setTimeout(() => {
      this.formResetToggle = true;
    });
  }
}
