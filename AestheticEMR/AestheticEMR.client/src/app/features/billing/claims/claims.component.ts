import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-claims',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="claims-container">
      <h2>Claims</h2>
      <p>Billing claims component - Coming Soon</p>
    </div>
  `,
  styles: [`
    .claims-container {
      padding: 20px;
    }

    @media (max-width: 992px) {
      .claims-container {
        padding: 16px;
      }
    }

    @media (max-width: 575.98px) {
      .claims-container {
        padding: 12px;
      }
    }
  `]
})
export class ClaimsComponent {

}
