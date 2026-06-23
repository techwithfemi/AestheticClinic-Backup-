import { Component, inject } from '@angular/core';
import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

// Material imports
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { fadeInOut } from '../../services/animations';
import { AccountService } from '../../services/account.service';
import { AlertService, MessageSeverity } from '../../services/alert.service';
import { Utilities } from '../../services/utilities';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.scss',
  animations: [fadeInOut],
  imports: [
    FormsModule,
    RouterLink,
    TranslateModule,
    NgClass,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ]
})
export class ChangePasswordComponent {
  private accountService = inject(AccountService);
  private alertService = inject(AlertService);

  currentPassword = '';
  newPassword = '';
  confirmPassword = '';
  isSaving = false;
  showValidationErrors = false;
  hideCurrentPassword = true;
  hideNewPassword = true;
  hideConfirmPassword = true;

  get passwordsDoNotMatch(): boolean {
    return !!this.newPassword && !!this.confirmPassword && this.newPassword !== this.confirmPassword;
  }

  togglePasswordVisibility(field: 'current' | 'new' | 'confirm') {
    switch (field) {
      case 'current':
        this.hideCurrentPassword = !this.hideCurrentPassword;
        break;
      case 'new':
        this.hideNewPassword = !this.hideNewPassword;
        break;
      case 'confirm':
        this.hideConfirmPassword = !this.hideConfirmPassword;
        break;
    }
  }

  showValidationAlerts() {
    this.showValidationErrors = true;

    if (!this.currentPassword) {
      this.alertService.showMessage('Current password is required', 'Please enter your current password', MessageSeverity.error);
      return;
    }

    if (!this.newPassword) {
      this.alertService.showMessage('New password is required', 'Please enter a new password', MessageSeverity.error);
      return;
    }

    if (this.newPassword.length < 6) {
      this.alertService.showMessage('Invalid new password', 'New password must be at least 6 characters', MessageSeverity.error);
      return;
    }

    if (!this.confirmPassword) {
      this.alertService.showMessage('Confirmation password is required', 'Please confirm your new password', MessageSeverity.error);
      return;
    }

    if (this.passwordsDoNotMatch) {
      this.alertService.showMessage('Password mismatch', 'New password and confirmation password do not match', MessageSeverity.error);
    }
  }

  save() {
    if (!this.currentPassword || !this.newPassword || !this.confirmPassword || this.newPassword.length < 6 || this.passwordsDoNotMatch) {
      this.showValidationAlerts();
      return;
    }

    this.showValidationErrors = true;
    this.isSaving = true;
    this.alertService.startLoadingMessage('', 'Changing password...');

    this.accountService.changePassword(this.currentPassword, this.newPassword, this.confirmPassword)
      .subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.isSaving = false;
          this.alertService.showMessage('Password Changed', 'Your password has been changed successfully.', MessageSeverity.success);
          this.currentPassword = '';
          this.newPassword = '';
          this.confirmPassword = '';
          this.showValidationErrors = false;
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.isSaving = false;

          const errorMessage = Utilities.getHttpResponseMessage(error) || 'Unable to change password.';
          const splitError = Utilities.splitInTwo(errorMessage, ':');
          const displayMessage = splitError.secondPart?.trim() || errorMessage;

          this.alertService.showStickyMessage(
            'Password Change Failed',
            displayMessage,
            MessageSeverity.error,
            error
          );
        }
      });
  }
}
