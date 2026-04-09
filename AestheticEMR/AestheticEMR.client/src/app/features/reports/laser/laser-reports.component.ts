import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-laser-reports',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="reports-container">
      <h2>Laser Reports</h2>
      <p>Laser reports component - Coming Soon</p>
    </div>
  `,
  styles: [`
    .reports-container {
      padding: 20px;
    }
  `]
})
export class LaserReportsComponent {

}
