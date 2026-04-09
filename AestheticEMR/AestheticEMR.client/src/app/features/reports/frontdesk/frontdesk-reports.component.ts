import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-frontdesk-reports',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="reports-container">
      <h2>Frontdesk Reports</h2>
      <p>Frontdesk reports component - Coming Soon</p>
    </div>
  `,
  styles: [`
    .reports-container {
      padding: 20px;
    }
  `]
})
export class FrontdeskReportsComponent {

}
