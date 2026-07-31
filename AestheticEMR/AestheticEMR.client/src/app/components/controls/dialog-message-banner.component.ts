import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, OnDestroy, Output, SimpleChanges } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

export type DialogMessageBannerType = 'success' | 'error' | 'warning' | 'info';

@Component({
  selector: 'app-dialog-message-banner',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule],
  templateUrl: './dialog-message-banner.component.html',
  styleUrl: './dialog-message-banner.component.scss'
})
export class DialogMessageBannerComponent implements OnChanges, OnDestroy {
  @Input() visible = false;
  @Input() title = '';
  @Input() message = '';
  @Input() type: DialogMessageBannerType = 'info';
  @Input() autoHideMs = 5000;

  @Output() visibleChange = new EventEmitter<boolean>();

  private hideTimeoutId: ReturnType<typeof setTimeout> | null = null;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['visible'] || changes['message'] || changes['type']) {
      this.scheduleAutoHide();
    }
  }

  ngOnDestroy(): void {
    this.clearHideTimeout();
  }

  close(): void {
    this.clearHideTimeout();
    this.visibleChange.emit(false);
  }

  iconName(): string {
    if (this.type === 'success') return 'check_circle';
    if (this.type === 'error') return 'error';
    if (this.type === 'warning') return 'warning';
    return 'info';
  }

  private scheduleAutoHide(): void {
    this.clearHideTimeout();

    if (!this.visible || this.autoHideMs <= 0) {
      return;
    }

    this.hideTimeoutId = setTimeout(() => {
      this.visibleChange.emit(false);
    }, this.autoHideMs);
  }

  private clearHideTimeout(): void {
    if (this.hideTimeoutId) {
      clearTimeout(this.hideTimeoutId);
      this.hideTimeoutId = null;
    }
  }
}
