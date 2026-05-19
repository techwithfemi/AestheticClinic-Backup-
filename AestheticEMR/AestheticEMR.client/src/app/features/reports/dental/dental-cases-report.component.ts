import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { DentalEndpoint } from '../../../services/dental-endpoint.service';
import { DentalChart } from '../../../models/dental.model';
import { OdontogramDialogComponent } from '../../dental/odontogram/odontogram-dialog.component';
import { AlertService, MessageSeverity } from '../../../services/alert.service';

@Component({
  selector: 'app-dental-cases-report',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, MatProgressSpinnerModule],
  templateUrl: './dental-cases-report.component.html',
  styleUrl: './dental-cases-report.component.scss'
})
export class DentalCasesReportComponent implements OnInit {
  private endpoint = inject(DentalEndpoint);
  private dialog = inject(MatDialog);
  private alertService = inject(AlertService);
  dentalCharts = signal<DentalChart[]>([]);
  loading = signal(false);
  displayedColumns = ['patientName', 'pno', 'consultId', 'tDate', 'inflammationOfGingiva', 'presenceOfDebris', 'presenceOfCalculus', 'presenceOfStains', 'underOrthodonticTreatment', 'actions'];

  ngOnInit(): void {
    this.loadCharts();
  }

  loadCharts(): void {
    this.loading.set(true);
    this.endpoint.getChartsEndpoint<DentalChart[]>().subscribe({
      next: charts => {
        this.dentalCharts.set(charts);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  editChart(chart: DentalChart): void {
    const dialogRef = this.dialog.open(OdontogramDialogComponent, {
      data: { chart, isEdit: true },
      width: '900px',
      disableClose: true
    });
    dialogRef.afterClosed().subscribe((result: any) => {
      if (result && result.chart) {
        this.endpoint.updateChartEndpoint<void>(result.chart.id, result.chart).subscribe({
          next: () => {
            this.alertService.showMessage('Dental record updated', '', MessageSeverity.success);
            this.loadCharts();
          },
          error: (err: any) => this.alertService.showStickyMessage('Update failed', err?.error?.message || 'Could not update record', MessageSeverity.error)
        });
      }
    });
  }

  deleteChart(chart: DentalChart): void {
    if (!confirm('Delete this dental record?')) return;
    this.endpoint.deleteChartEndpoint<void>(chart.id).subscribe({
      next: () => {
        this.alertService.showMessage('Dental record deleted', '', MessageSeverity.success);
        this.loadCharts();
      },
      error: (err: any) => this.alertService.showStickyMessage('Delete failed', err?.error?.message || 'Could not delete record', MessageSeverity.error)
    });
  }
}



