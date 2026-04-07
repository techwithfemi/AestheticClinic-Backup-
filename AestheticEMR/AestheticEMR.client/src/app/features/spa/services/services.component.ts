import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-services',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="services-container">
      <h2>Service Menu</h2>
      <p>Spa services component - Coming Soon</p>
    </div>
  `,
  styles: [`
    .services-container {
      padding: 20px;
    }
  `]
})
export class ServicesComponent {

}
