import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-incomes',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="page-shell">
      <h2>Incomes</h2>
      <p>Accounting incomes page - Coming Soon</p>
    </div>
  `,
  styles: [`
    .page-shell { padding: 20px; }
    @media (max-width: 992px) { .page-shell { padding: 16px; } }
    @media (max-width: 575.98px) { .page-shell { padding: 12px; } }
  `]
})
export class IncomesComponent {}
