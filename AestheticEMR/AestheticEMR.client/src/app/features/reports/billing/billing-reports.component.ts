import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-billing-reports',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="reports-container">
      <h2>Billing Reports</h2>
      <p>Billing reports component - Coming Soon</p>
    </div>
  `,
  styles: [`
    .reports-container {
      padding: 20px;
    }

    @media (max-width: 992px) {
      .reports-container {
        padding: 16px;
      }
    }

    @media (max-width: 575.98px) {
      .reports-container {
        padding: 12px;
      }
    }
  `]
})
export class BillingReportsComponent {

}
