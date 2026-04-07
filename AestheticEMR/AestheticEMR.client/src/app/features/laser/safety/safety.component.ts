import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-safety',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="safety-container">
      <h2>Safety Check</h2>
      <p>Laser safety check component - Coming Soon</p>
    </div>
  `,
  styles: [`
    .safety-container {
      padding: 20px;
    }
  `]
})
export class SafetyComponent {

}
