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
import { fadeInOut } from '../../services/animations';
import { AppointmentEndpoint } from '../../services/appointment-endpoint.service';
import { AttendanceEndpoint } from '../../services/attendance-endpoint.service';
import { HPatientEndpoint } from '../../services/h-patient-endpoint.service';
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
    TranslateModule
  ]
})
export class HomeComponent implements OnInit {
  private readonly appointmentEndpoint = inject(AppointmentEndpoint);
  private readonly attendanceEndpoint = inject(AttendanceEndpoint);
  private readonly patientEndpoint = inject(HPatientEndpoint);

  appointmentsToday: Appointment[] = [];
  attendanceToday: Attendance[] = [];
  patientRegistrationsToday: HPatient[] = [];
  readonly dashboardPageSize = 10;
  readonly appointmentColumns: string[] = ['patient', 'appointmentDate', 'appointmentTime', 'clinicType', 'remarks'];
  readonly attendanceColumns: string[] = ['date', 'consultId', 'patient', 'company', 'clinic', 'purpose'];
  readonly patientRegistrationColumns: string[] = ['patientNo', 'name', 'registrationDate', 'phone', 'email'];
  private patientsByNo = new Map<string, HPatient>();

  ngOnInit(): void {
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

  private getPatientName(patient: HPatient): string {
    return `${patient.pSurName ?? ''} ${patient.pFirstname ?? ''}`.trim() || (patient.pno ?? 'N/A');
  }

  private isToday(value?: string | null): boolean {
    if (!value) {
      return false;
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      return false;
    }

    const today = new Date();
    return date.getFullYear() === today.getFullYear()
      && date.getMonth() === today.getMonth()
      && date.getDate() === today.getDate();
  }

  private compareDateDesc(a?: string | null, b?: string | null): number {
    const aTime = a ? new Date(a).getTime() : 0;
    const bTime = b ? new Date(b).getTime() : 0;
    return bTime - aTime;
  }
}
