import { Directive, ElementRef, HostListener, inject } from '@angular/core';

@Directive({
  selector: 'mat-dialog-content[appDialogKeyboardScroll]',
  standalone: true
})
export class DialogKeyboardScrollDirective {
  private readonly elementRef = inject<ElementRef<HTMLElement>>(ElementRef);

  private lastKeyboardScrollTime = 0;
  private lastKeyboardScrollDirection: -1 | 0 | 1 = 0;
  private keyboardScrollStep = 0;

  @HostListener('document:keydown', ['$event'])
  handleKeyboardScroll(event: KeyboardEvent): void {
    if (event.key !== 'ArrowUp' && event.key !== 'ArrowDown') {
      return;
    }

    const contentElement = this.elementRef.nativeElement;
    const ownerDialog = this.getOwnerDialog(contentElement);
    const focusedElement = document.activeElement as HTMLElement | null;
    const eventTarget = event.target as HTMLElement | null;

    if (!this.isEventForOwnerDialog(ownerDialog, focusedElement, eventTarget)) {
      return;
    }

    const interactiveSelectors = ['input', 'select', 'textarea', 'button', '[contenteditable]'];

    if (focusedElement && interactiveSelectors.some(selector =>
      focusedElement.matches(selector) || !!focusedElement.closest(selector)
    )) {
      return;
    }

    if (focusedElement && (focusedElement.closest('mat-tab') || focusedElement.closest('.mat-mdc-select'))) {
      return;
    }

    event.preventDefault();
    event.stopPropagation();

    const direction: -1 | 1 = event.key === 'ArrowUp' ? -1 : 1;
    const now = typeof performance !== 'undefined' ? performance.now() : Date.now();
    const rapidRepeatThresholdMs = 120;
    const baseStep = 90;
    const accelerationStep = 45;
    const maxStep = 320;

    if (this.lastKeyboardScrollDirection !== direction || now - this.lastKeyboardScrollTime > rapidRepeatThresholdMs) {
      this.keyboardScrollStep = baseStep;
    } else {
      this.keyboardScrollStep = Math.min(maxStep, this.keyboardScrollStep + accelerationStep);
    }

    this.lastKeyboardScrollTime = now;
    this.lastKeyboardScrollDirection = direction;

    this.performStepScroll(direction * this.keyboardScrollStep);
  }

  private performStepScroll(distance: number): void {
    const contentElement = this.elementRef.nativeElement;
    const maxScrollTop = Math.max(0, contentElement.scrollHeight - contentElement.clientHeight);
    const next = Math.min(maxScrollTop, Math.max(0, contentElement.scrollTop + distance));
    contentElement.scrollTop = next;
  }

  private getOwnerDialog(element: HTMLElement): HTMLElement | null {
    return element.closest('mat-mdc-dialog-container, mat-dialog-container, [role="dialog"]');
  }

  private isEventForOwnerDialog(ownerDialog: HTMLElement | null, focusedElement: HTMLElement | null, eventTarget: HTMLElement | null): boolean {
    if (!ownerDialog) {
      return false;
    }

    if (focusedElement && ownerDialog.contains(focusedElement)) {
      return true;
    }

    if (eventTarget && ownerDialog.contains(eventTarget)) {
      return true;
    }

    return document.body === focusedElement && document.querySelector('mat-mdc-dialog-container:last-of-type, mat-dialog-container:last-of-type') === ownerDialog;
  }
}
