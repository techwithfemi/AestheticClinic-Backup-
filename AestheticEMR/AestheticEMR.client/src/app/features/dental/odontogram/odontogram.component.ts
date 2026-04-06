import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-odontogram',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="odontogram-container">
      <h2>Odontogram</h2>
      <p>Dental chart component - Coming Soon</p>
    </div>
  `,
  styles: [`
    .odontogram-container {
      padding: 20px;
    }
  `]
})
export class OdontogramComponent {

}
