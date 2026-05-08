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

interface PackageSummary {
  name: string;
  totalBookings: number;
  uniquePatients: number;
  lastBooked: string | undefined;
  bookingShare: number;
}

@Component({
  selector: 'app-spa-packages-report',
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
  templateUrl: './spa-packages-report.component.html',
  styleUrl: './spa-packages-report.component.scss'
})
export class SpaPackagesReportComponent {
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly alertService = inject(AlertService);

  loadingIndicator = false;
  readonly consultations = signal<AestheticConsultation[]>([]);

  searchText = '';
  dateFrom: Date | null = null;
  dateTo: Date | null = null;

  readonly displayedColumns = ['rank', 'package', 'bookings', 'patients', 'share', 'lastBooked'];

  private readonly dateFiltered = computed(() => {
    // "packages" are stored in the `services` field on the consultation
    let data = this.consultations().filter(c => c.services?.trim());

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

  readonly packageSummaries = computed((): PackageSummary[] => {
    const data = this.dateFiltered();
    const map = new Map<string, AestheticConsultation[]>();

    for (const c of data) {
      const key = c.services!.trim();
      const list = map.get(key) ?? [];
      list.push(c);
      map.set(key, list);
    }

    const total = data.length;
    const summaries: PackageSummary[] = [];
    for (const [name, sessions] of map.entries()) {
      const uniquePatients = new Set(sessions.map(s => s.pNo ?? s.patientId)).size;
      const sorted = sessions
        .map(s => s.consultationDate)
        .filter((d): d is string => !!d)
        .sort();
      summaries.push({
        name,
        totalBookings: sessions.length,
        uniquePatients,
        lastBooked: sorted[sorted.length - 1],
        bookingShare: total > 0 ? Math.round((sessions.length / total) * 100) : 0
      });
    }
    return summaries.sort((a, b) => b.totalBookings - a.totalBookings);
  });

  readonly filtered = computed(() => {
    const term = this.searchText.trim().toLowerCase();
    if (!term) return this.packageSummaries();
    return this.packageSummaries().filter(p => p.name.toLowerCase().includes(term));
  });

  readonly totalBookings    = computed(() => this.dateFiltered().length);
  readonly totalPackages    = computed(() => this.packageSummaries().length);
  readonly mostPopular      = computed(() => this.packageSummaries()[0]?.name ?? '—');
  readonly thisMonthBookings = computed(() => {
    const now = new Date();
    return this.consultations().filter(c => {
      if (!c.consultationDate || !c.services?.trim()) return false;
      const d = new Date(c.consultationDate);
      return d.getMonth() === now.getMonth() && d.getFullYear() === now.getFullYear();
    }).length;
  });

  constructor() { this.load(); }

  load(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading packages data...');
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
          `Unable to load packages report.\r\nError: "${error?.message ?? error}"`,
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
}



