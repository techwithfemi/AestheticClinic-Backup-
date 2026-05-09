import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class GlobalLoadingService {
  private readonly activeRequests = signal(0);
  readonly isLoading = signal(false);

  private showTimer: ReturnType<typeof setTimeout> | undefined;
  private hideTimer: ReturnType<typeof setTimeout> | undefined;

  startRequest(): void {
    this.activeRequests.update(count => count + 1);
    this.updateLoadingState();
  }

  endRequest(): void {
    this.activeRequests.update(count => Math.max(0, count - 1));
    this.updateLoadingState();
  }

  private updateLoadingState(): void {
    const hasActiveRequests = this.activeRequests() > 0;

    if (hasActiveRequests) {
      clearTimeout(this.hideTimer);

      if (!this.isLoading()) {
        clearTimeout(this.showTimer);
        this.showTimer = setTimeout(() => this.isLoading.set(true), 120);
      }

      return;
    }

    clearTimeout(this.showTimer);

    if (this.isLoading()) {
      clearTimeout(this.hideTimer);
      this.hideTimer = setTimeout(() => this.isLoading.set(false), 250);
    }
  }
}
