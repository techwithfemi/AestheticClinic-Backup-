import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-spa-reports',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="reports-container">
      <h2>Spa Reports</h2>
      <p>Spa reports component - Coming Soon</p>
    </div>
  `,
  styles: [`
    .reports-container {
      padding: 20px;
    }
  `]
})
export class SpaReportsComponent {

}
