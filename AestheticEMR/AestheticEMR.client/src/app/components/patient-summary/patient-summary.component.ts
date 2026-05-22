import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-patient-summary',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './patient-summary.component.html',
  styleUrl: './patient-summary.component.scss'
})
export class PatientSummaryComponent {
  @Input() photo?: string;
  @Input() fullName?: string;
  @Input() dateOfBirth?: string;
  @Input() companyName?: string;
  @Input() consultId?: string;
  @Input() clinic?: string;
  @Input() compact = false;

  get age(): number | null {
    if (!this.dateOfBirth) {
      return null;
    }

    const birthDate = new Date(this.dateOfBirth);
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

  get photoSource(): string {
    if (!this.photo) {
      return '';
    }

    return this.photo.startsWith('data:') ? this.photo : `data:image/jpeg;base64,${this.photo}`;
  }
}
