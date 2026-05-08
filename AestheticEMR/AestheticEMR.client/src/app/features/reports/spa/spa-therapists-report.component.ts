import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressBarModule } from '@angular/material/progress-bar';

import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AestheticConsultation } from '../../../models/aesthetic.model';
import { AlertService, MessageSeverity } from '../../../services/alert.service';

interface TherapistSummary {
  name: string;
  totalSessions: number;
  services: string[];
  lastSession: string | undefined;
  sessionShare: number;
}

@Component({
  selector: 'app-spa-therapists-report',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTooltipModule,
    MatProgressBarModule
  ],
  templateUrl: './spa-therapists-report.component.html',
  styleUrl: './spa-therapists-report.component.scss'
})
export class SpaTherapistsReportComponent {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly alertService = inject(AlertService);

  loadingIndicator = false;

  readonly consultations = signal<AestheticConsultation[]>([]);

  searchText = '';
  dateFrom: Date | null = null;
  dateTo: Date | null = null;

  readonly displayedColumns = ['rank', 'therapist', 'sessions', 'share', 'services', 'lastSession'];

  private readonly dateFiltered = computed(() => {
    let data = this.consultations().filter(c => c.provider?.trim());

    if (this.dateFrom) {
      const from = this.dateFrom.getTime();
      data = data.filter(c => c.consultationDate ? new Date(c.consultationDate).getTime() >= from : false);
    }
    if (this.dateTo) {
      const to = new Date(this.dateTo);
      to.setHours(23, 59, 59, 999);
      data = data.filter(c => c.consultationDate ? new Date(c.consultationDate).getTime() <= to.getTime() : false);
    }
    return data;
  });

  readonly therapistSummaries = computed((): TherapistSummary[] => {
    const data = this.dateFiltered();
    const map = new Map<string, AestheticConsultation[]>();
    for (const c of data) {
      const key = c.provider!.trim();
      const list = map.get(key) ?? [];
      list.push(c);
      map.set(key, list);
    }

    const total = data.length;
    const summaries: TherapistSummary[] = [];
    for (const [name, sessions] of map.entries()) {
      const services = [...new Set(sessions.map(s => s.indication ?? '').filter(s => s))];
      const sorted = sessions
        .map(s => s.consultationDate)
        .filter((d): d is string => !!d)
        .sort();
      summaries.push({
        name,
        totalSessions: sessions.length,
        services,
        lastSession: sorted[sorted.length - 1],
        sessionShare: total > 0 ? Math.round((sessions.length / total) * 100) : 0
      });
    }

    return summaries.sort((a, b) => b.totalSessions - a.totalSessions);
  });

  readonly filtered = computed(() => {
    const term = this.searchText.trim().toLowerCase();
    if (!term) return this.therapistSummaries();
    return this.therapistSummaries().filter(t => t.name.toLowerCase().includes(term));
  });

  readonly totalSessions    = computed(() => this.dateFiltered().length);
  readonly totalTherapists  = computed(() => this.therapistSummaries().length);
  readonly topTherapist     = computed(() => this.therapistSummaries()[0]?.name ?? '—');
  readonly avgSessionsPerTherapist = computed(() => {
    const count = this.therapistSummaries().length;
    return count > 0 ? Math.round(this.dateFiltered().length / count) : 0;
  });

  constructor() { this.load(); }

  load(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading therapist data...');
    this.endpoint.getSpaConsultationsEndpoint<AestheticConsultation[]>().subscribe({
      next: data => {
        this.consultations.set(data ?? []);
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage(
          'Load Error',
          `Unable to load therapist report.\r\nError: "${error?.message ?? error}"`,
          MessageSeverity.error,
          error
        );
      }
    });
  }

  clearFilters(): void {
    this.searchText = '';
    this.dateFrom = null;
    this.dateTo = null;
  }

  printReport(): void { window.print(); }

  getBarColor(share: number): string {
    if (share >= 40) return 'accent';
    if (share >= 20) return 'primary';
    return 'warn';
  }
}



