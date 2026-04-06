import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-invoices',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="invoices-container">
      <h2>Invoices</h2>
      <p>Billing invoices component - Coming Soon</p>
    </div>
  `,
  styles: [`
    .invoices-container {
      padding: 20px;
    }
  `]
})
export class InvoicesComponent {

}
