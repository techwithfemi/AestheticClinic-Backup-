import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

import { VwhRecord } from '../../models/legacy/vwh-record.model';

@Component({
  selector: 'app-attendance-summary',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './attendance-summary.component.html',
  styleUrl: './attendance-summary.component.scss'
})
export class AttendanceSummaryComponent {
  @Input() attendance?: VwhRecord;
  @Input() photo?: string;
  @Input() compact = false;

  get age(): number | null {
    if (typeof this.attendance?.age === 'number' && this.attendance.age >= 0) {
      return this.attendance.age;
    }

    const dob = this.attendance?.dob;
    if (!dob) {
      return null;
    }

    const birthDate = new Date(dob);
    if (Number.isNaN(birthDate.getTime())) {
      return null;
    }

    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();

    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }

    return age >= 0 ? age : null;
  }

  get companyName(): string {
    return this.attendance?.retainName?.trim() || this.attendance?.coyname?.trim() || '—';
  }

  get photoSource(): string {
    if (!this.photo) {
      return '';
    }

    return this.photo.startsWith('data:') ? this.photo : `data:image/jpeg;base64,${this.photo}`;
  }
}
