import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-employees-department-report',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="reports-container">
      <h2>Employees Department Report</h2>
      <p>Employees department report - Coming Soon</p>
    </div>
  `,
  styles: [`.reports-container { padding: 20px; } @media (max-width: 992px) { .reports-container { padding: 16px; } } @media (max-width: 575.98px) { .reports-container { padding: 12px; } }`]
})
export class EmployeesDepartmentReportComponent {}
