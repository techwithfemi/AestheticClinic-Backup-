// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { TranslateModule } from '@ngx-translate/core';

import { Appointment } from '../../models/legacy/appointment.model';
import { Attendance } from '../../models/legacy/attendance.model';
import { HPatient } from '../../models/legacy/h-patient.model';
import { HRetainership } from '../../models/legacy/h-retainership.model';
import { fadeInOut } from '../../services/animations';
import { AppointmentEndpoint } from '../../services/appointment-endpoint.service';
import { AttendanceEndpoint } from '../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../services/h-patient-endpoint.service';
import { HRetainershipEndpoint } from '../../services/h-retainership-endpoint.service';
import { UtcDisplayPipe } from '../../pipes/utc-display.pipe';
import { StatisticsDemoComponent } from '../controls/statistics-demo.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
  animations: [fadeInOut],
  imports: [
    CommonModule,
    StatisticsDemoComponent,
    MatTableModule,
    MatToolbarModule,
    TranslateModule,
    UtcDisplayPipe
  ]
})
export class HomeComponent implements OnInit {
  private readonly appointmentEndpoint = inject(AppointmentEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly patientEndpoint = inject(HPatientEndpoint);
  private readonly retainershipEndpoint = inject(HRetainershipEndpoint);

  appointmentsToday: Appointment[] = [];
  attendanceToday: Attendance[] = [];
  patientRegistrationsToday: HPatient[] = [];
  readonly dashboardPageSize = 10;
  readonly appointmentColumns: string[] = ['patient', 'appointmentDate', 'appointmentTime', 'clinicType', 'remarks'];
  readonly attendanceColumns: string[] = ['date', 'consultId', 'patient', 'company', 'clinic', 'purpose'];
  readonly patientRegistrationColumns: string[] = ['patientNo', 'name', 'registrationDate', 'phone', 'email'];
  private patientsByNo = new Map<string, HPatient>();
  private companiesByRetainId = new Map<string, string>();

  ngOnInit(): void {
    this.loadRetainerships();
    this.loadPatients();
    this.loadAppointmentsToday();
    this.loadAttendanceToday();
    this.loadPatientRegistrationsToday();
  }

  getPatientNameByNo(pNo?: string): string {
    if (!pNo) {
      return 'N/A';
    }

    const patient = this.patientsByNo.get(pNo);
    if (!patient) {
      return pNo;
    }

    return this.getPatientName(patient);
  }

  getCompanyNameByRetainId(retainId?: string): string {
    if (!retainId) {
      return 'N/A';
    }

    const companyName = this.companiesByRetainId.get(retainId);
    return companyName || retainId;
  }

  private loadPatients(): void {
    this.patientEndpoint.getHPatientsEndpoint<HPatient[]>()
      .subscribe({
        next: patients => {
          this.patientsByNo = new Map(patients
            .filter(patient => !!patient.pno)
            .map(patient => [patient.pno as string, patient]));
        },
        error: () => {
          this.patientsByNo = new Map<string, HPatient>();
        }
      });
  }

  private loadAppointmentsToday(): void {
    this.appointmentEndpoint.getAppointmentsEndpoint<Appointment[]>()
      .subscribe({
        next: appointments => {
          this.appointmentsToday = appointments
            .filter(item => this.isToday(item.apptDate))
            .sort((a, b) => this.compareDateDesc(a.apptTime, b.apptTime))
            .slice(0, this.dashboardPageSize);
        },
        error: () => {
          this.appointmentsToday = [];
        }
      });
  }

  private loadAttendanceToday(): void {
    this.attendanceEndpoint.getAttendancesEndpoint<Attendance[]>()
      .subscribe({
        next: attendances => {
          this.attendanceToday = attendances
            .filter(item => this.isToday(item.recDate))
            .sort((a, b) => this.compareDateDesc(a.entryDate ?? a.recDate, b.entryDate ?? b.recDate))
            .slice(0, this.dashboardPageSize);
        },
        error: () => {
          this.attendanceToday = [];
        }
      });
  }

  private loadPatientRegistrationsToday(): void {
    this.patientEndpoint.getHPatientsEndpoint<HPatient[]>()
      .subscribe({
        next: patients => {
          this.patientRegistrationsToday = patients
            .filter(item => this.isToday(item.regDate))
            .sort((a, b) => this.compareDateDesc(a.regDate, b.regDate))
            .slice(0, this.dashboardPageSize);
        },
        error: () => {
          this.patientRegistrationsToday = [];
        }
      });
  }

  private loadRetainerships(): void {
    this.retainershipEndpoint.getHRetainershipsEndpoint<HRetainership[]>()
      .subscribe({
        next: retainerships => {
          this.companiesByRetainId = new Map(retainerships
            .filter(r => !!r.retainId)
            .map(r => [r.retainId, r.retainName || r.retainId]));
        },
        error: () => {
          this.companiesByRetainId = new Map<string, string>();
        }
      });
  }

  private getPatientName(patient: HPatient): string {
    return `${patient.pSurName ?? ''} ${patient.pFirstname ?? ''}`.trim() || (patient.pno ?? 'N/A');
  }

  private isToday(value?: string | null): boolean {
    const valueKey = this.toLocalDateKey(value);
    if (!valueKey) {
      return false;
    }

    return valueKey === this.toLocalDateKey(new Date().toISOString());
  }

  private compareDateDesc(a?: string | null, b?: string | null): number {
    return this.toComparableTime(b) - this.toComparableTime(a);
  }

  private toComparableTime(value?: string | null): number {
    if (!value) {
      return 0;
    }

    const text = value.trim();
    if (!text) {
      return 0;
    }

    const ymdOnly = text.match(/^(\d{4})-(\d{2})-(\d{2})$/);
    if (ymdOnly) {
      const [, y, m, d] = ymdOnly;
      return new Date(Number(y), Number(m) - 1, Number(d)).getTime();
    }

    const dmyOnly = text.match(/^(\d{1,2})[\/-](\d{1,2})[\/-](\d{4})$/);
    if (dmyOnly) {
      const [, d, m, y] = dmyOnly;
      return new Date(Number(y), Number(m) - 1, Number(d)).getTime();
    }

    const parsed = new Date(text);
    return Number.isNaN(parsed.getTime()) ? 0 : parsed.getTime();
  }

  private toLocalDateKey(value?: string | null): string | null {
    if (!value) {
      return null;
    }

    const text = value.trim();
    if (!text) {
      return null;
    }

    const ymd = text.match(/^(\d{4})-(\d{2})-(\d{2})/);
    if (ymd) {
      return `${ymd[1]}-${ymd[2]}-${ymd[3]}`;
    }

    const dmy = text.match(/^(\d{1,2})[\/-](\d{1,2})[\/-](\d{4})$/);
    if (dmy) {
      const day = dmy[1].padStart(2, '0');
      const month = dmy[2].padStart(2, '0');
      return `${dmy[3]}-${month}-${day}`;
    }

    const parsed = new Date(text);
    if (Number.isNaN(parsed.getTime())) {
      return null;
    }

    const year = parsed.getFullYear();
    const month = String(parsed.getMonth() + 1).padStart(2, '0');
    const day = String(parsed.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
}
