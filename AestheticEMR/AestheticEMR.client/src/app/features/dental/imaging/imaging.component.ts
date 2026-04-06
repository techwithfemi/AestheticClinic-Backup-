import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-imaging',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="imaging-container">
      <h2>Imaging</h2>
      <p>Dental imaging component - Coming Soon</p>
    </div>
  `,
  styles: [`
    .imaging-container {
      padding: 20px;
    }
  `]
})
export class ImagingComponent {

}
