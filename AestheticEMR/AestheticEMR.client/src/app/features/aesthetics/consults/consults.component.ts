import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-consults',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="consults-container">
      <h2>Consultations</h2>
      <p>Aesthetics consultations component - Coming Soon</p>
    </div>
  `,
  styles: [`
    .consults-container {
      padding: 20px;
    }
  `]
})
export class ConsultsComponent {

}
