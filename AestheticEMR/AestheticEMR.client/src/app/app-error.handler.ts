// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

import { Injectable, ErrorHandler } from '@angular/core';

@Injectable()
export class AppErrorHandler extends ErrorHandler {
  constructor() {
    super();
  }

  override handleError(error: Error) {
    const message = error?.message || '';

    // Prevent endless blocking dialog loop for known Angular Material template/runtime error.
    if (message.includes('mat-form-field must contain a MatFormFieldControl')) {
      console.error('Runtime template error (suppressed fatal popup):', error);
      super.handleError(error);
      return;
    }

    if (confirm("Fatal Error!\nAn unresolved error has occurred. Do you want to reload the page to correct this?\n\n" +
      `Error: ${message}`)) {
      window.location.reload();
    }

    super.handleError(error);
  }
}
