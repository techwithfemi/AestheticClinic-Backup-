import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-employees-info-report',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="reports-container">
      <h2>Employees Info Report</h2>
      <p>Employees info report - Coming Soon</p>
    </div>
  `,
  styles: [`.reports-container { padding: 20px; } @media (max-width: 992px) { .reports-container { padding: 16px; } } @media (max-width: 575.98px) { .reports-container { padding: 12px; } }`]
})
export class EmployeesInfoReportComponent {}
