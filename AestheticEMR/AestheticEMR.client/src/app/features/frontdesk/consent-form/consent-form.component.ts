import { Component, inject } from '@angular/core';
import { ConsentFormListComponent } from './consent-form-list.component';

@Component({
  selector: 'app-consent-form',
  standalone: true,
  imports: [ConsentFormListComponent],
  template: `<app-consent-form-list></app-consent-form-list>`
})
export class ConsentFormComponent {
  // Delegates to the new list component which manages add/edit via dialog
}
