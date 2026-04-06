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
  `]
})
export class ClaimsComponent {

}
