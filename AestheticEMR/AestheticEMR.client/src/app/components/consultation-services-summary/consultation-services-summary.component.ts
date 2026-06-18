import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

import { ConsultingDetailsForBilling } from '../../models/legacy/consulting-details-for-billing.model';

@Component({
  selector: 'app-consultation-services-summary',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './consultation-services-summary.component.html',
  styleUrl: './consultation-services-summary.component.scss'
})
export class ConsultationServicesSummaryComponent {
  @Input() title = 'Consultation / Services Rendered';
  @Input() items: ConsultingDetailsForBilling[] = [];

  hasValue(value?: string | null): boolean {
    return !!(value ?? '').trim();
  }

  getConsultationLine(item: ConsultingDetailsForBilling): string {
    const treatedBy = (item.treatedBy ?? '').trim();
    const date = this.formatDate(item.cDate);
    const time = (item.cTime ?? '').trim();

    const segments = [
      treatedBy ? `Treatment by Dr. ${treatedBy}` : '',
      date ? `on ${date}` : '',
      time
    ].filter(x => !!x);

    return segments.join(' ');
  }

  getListLines(value?: string | null): string[] {
    return (value ?? '')
      .split(/\r?\n/)
      .map(x => x.trim())
      .filter(x => !!x)
      .map(x => x.replace(/^[-•]\s*/, '').trim())
      .filter(x => !!x);
  }

  formatDate(value?: string | null): string {
    const source = (value ?? '').trim();
    if (!source) {
      return '';
    }

    const date = new Date(source);
    if (Number.isNaN(date.getTime())) {
      return source;
    }

    return date.toLocaleDateString('en-GB', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }
}
