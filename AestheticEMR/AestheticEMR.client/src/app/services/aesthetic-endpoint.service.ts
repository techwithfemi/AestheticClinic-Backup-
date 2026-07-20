// ---------------------------------------
// Aesthetic EMR frontend endpoint service
// ---------------------------------------

import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';

export interface ProfitAndLossHeader {
  itemName?: string | null;
  groupID: string;
}

export interface BalanceSheetHeader {
  itemName?: string | null;
  rptType?: string | null;
  period: string;
  coyID: string;
}

export interface AccountingReportYear {
  periodYr: string;
}

export interface AccountingReportPeriod {
  period: string;
  periodVal?: string | null;
  isClose: boolean;
  prdClose: string;
}

export interface AccountingLedgerLookup {
  ledgerCode: string;
  ledger: string;
}

export interface AccountingAccountLookup {
  accountNo: string;
  accountName: string;
}

export interface AccountingReportDefaults {
  coyID: string;
}

@Injectable({
  providedIn: 'root'
})
export class AestheticEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get baseUrl() { return `${this.configurations.baseUrl}/api/aesthetic`; }
  private get auditUrl() { return `${this.configurations.baseUrl}/api/audit`; }
  private get accountingReportsUrl() { return `${this.configurations.baseUrl}/api/accounting/reports`; }

  private get patientsUrl() { return `${this.baseUrl}/patients`; }
  private get consultationsUrl() { return `${this.baseUrl}/consultations`; }
  private get photosUrl() { return `${this.baseUrl}/photos`; }
  private get botoxConsultationsUrl() { return `${this.consultationsUrl}/botox`; }

  getAccountingReportDefaultsEndpoint(): Observable<AccountingReportDefaults> {
    return this.http.get<AccountingReportDefaults>(`${this.accountingReportsUrl}/defaults`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAccountingReportDefaultsEndpoint())));
  }

  getAccountingGeneralLedgerYearsEndpoint(): Observable<AccountingReportYear[]> {
    return this.http.get<AccountingReportYear[]>(`${this.accountingReportsUrl}/general-ledger/years`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAccountingGeneralLedgerYearsEndpoint())));
  }

  getAccountingGeneralLedgerPeriodsEndpoint(coyID: string, year: string): Observable<AccountingReportPeriod[]> {
    const query = new URLSearchParams({ coyID, year });

    return this.http.get<AccountingReportPeriod[]>(`${this.accountingReportsUrl}/general-ledger/periods?${query.toString()}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAccountingGeneralLedgerPeriodsEndpoint(coyID, year))));
  }

  getAccountingGeneralLedgerLedgersEndpoint(): Observable<AccountingLedgerLookup[]> {
    return this.http.get<AccountingLedgerLookup[]>(`${this.accountingReportsUrl}/general-ledger/ledgers`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAccountingGeneralLedgerLedgersEndpoint())));
  }

  getAccountingGeneralLedgerAccountsEndpoint(coyID: string, period: string, ledgerCode: string): Observable<AccountingAccountLookup[]> {
    const query = new URLSearchParams({ coyID, period, ledgerCode });

    return this.http.get<AccountingAccountLookup[]>(`${this.accountingReportsUrl}/general-ledger/accounts?${query.toString()}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAccountingGeneralLedgerAccountsEndpoint(coyID, period, ledgerCode))));
  }

  getAccountingGeneralLedgerReportEndpoint(params: { coyID: string; period: string; ledgerCode: string; accountNo: string; ledgerDisplayText?: string; accountDisplayText?: string }): Observable<Blob> {
    const query = new URLSearchParams({
      coyID: params.coyID,
      period: params.period,
      ledgerCode: params.ledgerCode,
      accountNo: params.accountNo
    });

    if (params.ledgerDisplayText) {
      query.set('ledgerDisplayText', params.ledgerDisplayText);
    }

    if (params.accountDisplayText) {
      query.set('accountDisplayText', params.accountDisplayText);
    }

    return this.http.get(`${this.accountingReportsUrl}/general-ledger?${query.toString()}`, {
      ...this.requestHeaders,
      responseType: 'blob'
    }).pipe(
      catchError(error => this.handleError(error, () => this.getAccountingGeneralLedgerReportEndpoint(params)))) as Observable<Blob>;
  }

  getAccountingProfitAndLossReportEndpoint(params: { coyID: string; period: string; year: string; rptBy: string; isClose: boolean }): Observable<Blob> {
    const query = new URLSearchParams({
      coyID: params.coyID,
      period: params.period,
      year: params.year,
      rptBy: params.rptBy,
      isClose: String(params.isClose)
    });

    return this.http.get(`${this.accountingReportsUrl}/profit-and-loss?${query.toString()}`, {
      ...this.requestHeaders,
      responseType: 'blob'
    }).pipe(
      catchError(error => this.handleError(error, () => this.getAccountingProfitAndLossReportEndpoint(params)))) as Observable<Blob>;
  }

  getAccountingProfitAndLossHeadersEndpoint(): Observable<ProfitAndLossHeader[]> {
    return this.http.get<ProfitAndLossHeader[]>(`${this.accountingReportsUrl}/profit-and-loss/headers`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAccountingProfitAndLossHeadersEndpoint())));
  }

  getAccountingBalanceSheetHeadersEndpoint(): Observable<BalanceSheetHeader[]> {
    return this.http.get<BalanceSheetHeader[]>(`${this.accountingReportsUrl}/balance-sheet/headers`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAccountingBalanceSheetHeadersEndpoint())));
  }

  getAccountingProfitAndLossDetailsReportEndpoint(params: { coyID: string; period: string; year: string; rptBy: string; groupID: string; isClose: boolean }): Observable<Blob> {
    const query = new URLSearchParams({
      coyID: params.coyID,
      period: params.period,
      year: params.year,
      rptBy: params.rptBy,
      groupID: params.groupID,
      isClose: String(params.isClose)
    });

    return this.http.get(`${this.accountingReportsUrl}/profit-and-loss/details?${query.toString()}`, {
      ...this.requestHeaders,
      responseType: 'blob'
    }).pipe(
      catchError(error => this.handleError(error, () => this.getAccountingProfitAndLossDetailsReportEndpoint(params)))) as Observable<Blob>;
  }

  getAccountingBalanceSheetReportEndpoint(params: { coyID: string; period: string; year: string; rptBy: string; isClose: boolean }): Observable<Blob> {
    const query = new URLSearchParams({
      coyID: params.coyID,
      period: params.period,
      year: params.year,
      rptBy: params.rptBy,
      isClose: String(params.isClose)
    });

    return this.http.get(`${this.accountingReportsUrl}/balance-sheet?${query.toString()}`, {
      ...this.requestHeaders,
      responseType: 'blob'
    }).pipe(
      catchError(error => this.handleError(error, () => this.getAccountingBalanceSheetReportEndpoint(params)))) as Observable<Blob>;
  }

  getPatientsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.patientsUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getPatientsEndpoint<T>())));
  }

  getPatientConsultationsEndpoint<T>(patientId: number): Observable<T> {
    return this.http.get<T>(`${this.patientsUrl}/${patientId}/consultations`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getPatientConsultationsEndpoint<T>(patientId))));
  }

  createPatientEndpoint<T>(patient: object): Observable<T> {
    return this.http.post<T>(this.patientsUrl, JSON.stringify(patient), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createPatientEndpoint<T>(patient))));
  }

  createConsultationEndpoint<T>(consultation: object): Observable<T> {
    return this.http.post<T>(this.consultationsUrl, JSON.stringify(consultation), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createConsultationEndpoint<T>(consultation))));
  }

  getBotoxConsultationsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.botoxConsultationsUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getBotoxConsultationsEndpoint<T>())));
  }

  createBotoxConsultationEndpoint<T>(consultation: object): Observable<T> {
    return this.http.post<T>(this.botoxConsultationsUrl, JSON.stringify(consultation), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createBotoxConsultationEndpoint<T>(consultation))));
  }

  getLaserConsultationsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.consultationsUrl}/laser`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getLaserConsultationsEndpoint<T>())));
  }

  createLaserConsultationEndpoint<T>(consultation: object): Observable<T> {
    return this.http.post<T>(`${this.consultationsUrl}/laser`, JSON.stringify(consultation), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createLaserConsultationEndpoint<T>(consultation))));
  }

  getSpaConsultationsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.consultationsUrl}/spa`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getSpaConsultationsEndpoint<T>())));
  }

  createSpaConsultationEndpoint<T>(consultation: object): Observable<T> {
    return this.http.post<T>(`${this.consultationsUrl}/spa`, JSON.stringify(consultation), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createSpaConsultationEndpoint<T>(consultation))));
  }

  updateConsultationEndpoint<T>(consultationId: number, consultation: object): Observable<T> {
    return this.http.put<T>(`${this.consultationsUrl}/${consultationId}`, JSON.stringify(consultation), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.updateConsultationEndpoint<T>(consultationId, consultation))));
  }

  deleteConsultationEndpoint<T>(consultationId: number): Observable<T> {
    return this.http.delete<T>(`${this.consultationsUrl}/${consultationId}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.deleteConsultationEndpoint<T>(consultationId))));
  }

  getConsentTemplatesEndpoint<T>(procedureType: string, includeInactive = false): Observable<T> {
    const params = new URLSearchParams();
    if (procedureType) {
      params.set('procedureType', procedureType);
    }
    if (includeInactive) {
      params.set('includeInactive', 'true');
    }

    return this.http.get<T>(`${this.baseUrl}/consent-templates${params.toString() ? `?${params.toString()}` : ''}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getConsentTemplatesEndpoint<T>(procedureType, includeInactive))));
  }

  getConsentTemplateEndpoint<T>(id: number): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/consent-templates/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getConsentTemplateEndpoint<T>(id))));
  }

  createConsentTemplateEndpoint<T>(template: object): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}/consent-templates`, JSON.stringify(template), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createConsentTemplateEndpoint<T>(template))));
  }

  updateConsentTemplateEndpoint<T>(id: number, template: object): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/consent-templates/${id}`, JSON.stringify(template), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.updateConsentTemplateEndpoint<T>(id, template))));
  }

  deleteConsentTemplateEndpoint<T>(id: number): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}/consent-templates/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.deleteConsentTemplateEndpoint<T>(id))));
  }

  getConsentStatusEndpoint<T>(consultId: string, pNo: string, procedureType: string): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/consent-status?consultId=${encodeURIComponent(consultId)}&pNo=${encodeURIComponent(pNo)}&procedureType=${encodeURIComponent(procedureType)}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getConsentStatusEndpoint<T>(consultId, pNo, procedureType))));
  }

  signConsentEndpoint<T>(payload: object): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}/consents/sign`, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.signConsentEndpoint<T>(payload))));
  }

  markConsentViewedEndpoint<T>(consentId: number): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/consents/${consentId}/viewed`, JSON.stringify({}), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.markConsentViewedEndpoint<T>(consentId))));
  }

  getSignedConsentsEndpoint<T>(options?: { consultId?: string; pNo?: string; procedureType?: string; includeVoided?: boolean }): Observable<T> {
    const params = new URLSearchParams();
    if (options?.consultId) params.set('consultId', options.consultId);
    if (options?.pNo) params.set('pNo', options.pNo);
    if (options?.procedureType) params.set('procedureType', options.procedureType);
    if (options?.includeVoided !== undefined) params.set('includeVoided', String(options.includeVoided));
    const query = params.toString();
    const url = query ? `${this.baseUrl}/consents?${query}` : `${this.baseUrl}/consents`;

    return this.http.get<T>(url, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getSignedConsentsEndpoint<T>(options))));
  }

  getSignedConsentEndpoint<T>(consentId: number): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/consents/${consentId}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getSignedConsentEndpoint<T>(consentId))));
  }

  voidConsentEndpoint<T>(consentId: number, payload: object): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/consents/${consentId}/void`, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.voidConsentEndpoint<T>(consentId, payload))));
  }

  updateSignedConsentEndpoint<T>(consentId: number, payload: object): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/consents/${consentId}`, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.updateSignedConsentEndpoint<T>(consentId, payload))));
  }

  getPhotosEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.photosUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getPhotosEndpoint<T>())));
  }

  createPhotoEndpoint<T>(photo: object): Observable<T> {
    return this.http.post<T>(this.photosUrl, JSON.stringify(photo), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.createPhotoEndpoint<T>(photo))));
  }

  uploadPhotoEndpoint<T>(payload: FormData): Observable<T> {
    return this.http.post<T>(`${this.photosUrl}/upload`, payload, this.uploadHeaders).pipe(
      catchError(error => this.handleError(error, () => this.uploadPhotoEndpoint<T>(payload))));
  }

  updatePhotoEndpoint<T>(photoId: number, photo: object): Observable<T> {
    return this.http.put<T>(`${this.photosUrl}/${photoId}`, JSON.stringify(photo), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.updatePhotoEndpoint<T>(photoId, photo))));
  }

  updatePhotoUploadEndpoint<T>(photoId: number, payload: FormData): Observable<T> {
    return this.http.put<T>(`${this.photosUrl}/${photoId}/upload`, payload, this.uploadHeaders).pipe(
      catchError(error => this.handleError(error, () => this.updatePhotoUploadEndpoint<T>(photoId, payload))));
  }

  deletePhotoEndpoint<T>(photoId: number): Observable<T> {
    return this.http.delete<T>(`${this.photosUrl}/${photoId}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.deletePhotoEndpoint<T>(photoId))));
  }

  getFollowUpsEndpoint<T>(options?: { patientId?: number; consultationId?: number; isCompleted?: boolean }): Observable<T> {
    const params = new URLSearchParams();
    if (options?.patientId !== undefined) params.set('patientId', String(options.patientId));
    if (options?.consultationId !== undefined) params.set('consultationId', String(options.consultationId));
    if (options?.isCompleted !== undefined) params.set('isCompleted', String(options.isCompleted));
    const query = params.toString();
    const url = query ? `${this.baseUrl}/follow-ups?${query}` : `${this.baseUrl}/follow-ups`;

    return this.http.get<T>(url, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getFollowUpsEndpoint<T>(options))));
  }

  scheduleFollowUpEndpoint<T>(payload: object): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}/follow-ups/schedule`, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.scheduleFollowUpEndpoint<T>(payload))));
  }

  sendPatientSatisfactionEmailEndpoint<T>(followUpId: number, payload: object): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}/follow-ups/${followUpId}/patient-satisfaction/send`, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.sendPatientSatisfactionEmailEndpoint<T>(followUpId, payload))));
  }

  getPatientSatisfactionSurveyEndpoint<T>(token: string): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/patient-satisfaction?token=${encodeURIComponent(token)}`).pipe(
      catchError(error => this.handleError(error, () => this.getPatientSatisfactionSurveyEndpoint<T>(token))));
  }

  submitPatientSatisfactionEndpoint<T>(token: string, payload: object): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}/patient-satisfaction/submit?token=${encodeURIComponent(token)}`, JSON.stringify(payload), this.jsonHeadersWithoutAuth).pipe(
      catchError(error => this.handleError(error, () => this.submitPatientSatisfactionEndpoint<T>(token, payload))));
  }

  getOpenAuditIncidentsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(`${this.auditUrl}/incidents/open`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getOpenAuditIncidentsEndpoint<T>())));
  }

  getAuditIncidentsEndpoint<T>(severity?: string, fromDate?: string, toDate?: string): Observable<T> {
    const params = new URLSearchParams();
    if (severity) params.set('severity', severity);
    if (fromDate) params.set('fromDate', fromDate);
    if (toDate) params.set('toDate', toDate);

    const query = params.toString();
    const url = query ? `${this.auditUrl}/incidents?${query}` : `${this.auditUrl}/incidents`;

    return this.http.get<T>(url, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAuditIncidentsEndpoint<T>(severity, fromDate, toDate))));
  }

  getConsultationAuditTrailEndpoint<T>(consultationId: number): Observable<T> {
    return this.http.get<T>(`${this.auditUrl}/consultation/${consultationId}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getConsultationAuditTrailEndpoint<T>(consultationId))));
  }

  reviewAuditIncidentEndpoint<T>(auditLogId: number, payload: object): Observable<T> {
    return this.http.put<T>(`${this.auditUrl}/${auditLogId}/review`, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.reviewAuditIncidentEndpoint<T>(auditLogId, payload))));
  }

  getStaffRosterReportEndpoint(params: { coyID: string; month: string; year: string; deptID: string; isClose: boolean }): Observable<Blob> {
    const query = new URLSearchParams({
      coyID: params.coyID,
      month: params.month,
      year: params.year,
      deptID: params.deptID,
      isClose: String(params.isClose)
    });

    return this.http.get(`${this.accountingReportsUrl}/staffroster/roster?${query.toString()}`, {
      ...this.requestHeaders,
      responseType: 'blob'
    }).pipe(
      catchError(error => this.handleError(error, () => this.getStaffRosterReportEndpoint(params)))
    ) as Observable<Blob>;
  }

  private get jsonHeadersWithoutAuth(): { headers: HttpHeaders | Record<string, string | string[]> } {
    return {
      headers: {
        'Content-Type': 'application/json',
        Accept: 'application/json, text/plain, */*'
      }
    };
  }
}
