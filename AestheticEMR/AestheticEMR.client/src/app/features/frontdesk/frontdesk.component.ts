import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-frontdesk',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="frontdesk-container">
      <h2>Front Desk</h2>
      <p>Front desk management component - Coming Soon</p>
    </div>
  `,
  styles: [`
    .frontdesk-container {
      padding: 20px;
    }
  `]
})
export class FrontdeskComponent {

}
