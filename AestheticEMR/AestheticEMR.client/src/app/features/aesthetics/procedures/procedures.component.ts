import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';

import { AlertService, MessageSeverity } from '../../../services/alert.service';
import { AestheticEndpoint } from '../../../services/aesthetic-endpoint.service';
import { AestheticConsultation, AestheticPatient, AestheticPhoto } from '../../../models/aesthetic.model';

type PhotoTab = 'neuromodulator' | 'dermalFiller' | 'laser';
type PhotoPhase = 'Before' | 'After';
type StandardTag = 'frontal' | 'left' | 'right' | 'profile';

interface TabPhotoItem {
  id?: number;
  consultationId?: number;
  fileName: string;
  phase: PhotoPhase;
  tag: StandardTag;
  url?: string;
  file?: File;
}

@Component({
  selector: 'app-procedures',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTabsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatSlideToggleModule,
    MatCheckboxModule,
    MatIconModule,
    MatCardModule
  ],
  template: `
    <div class="procedures-page">
      <div class="page-header">
        <div>
          <h2>Aesthetic Procedures</h2>
          <p class="subtitle">Unified consultation and procedure tabs with single save/update action.</p>
        </div>
      </div>

      <mat-card>
        <form [formGroup]="form" class="form-shell">
          <div class="patient-row">
            <mat-form-field appearance="outline" class="patient-field">
              <mat-label>Patient</mat-label>
              <mat-select formControlName="patientId" (selectionChange)="onPatientChanged()">
                @for (p of patients(); track p.id) {
                  <mat-option [value]="p.id">{{ p.firstName }} {{ p.lastName }}{{ p.pno ? ' [' + p.pno + ']' : '' }}</mat-option>
                }
              </mat-select>
            </mat-form-field>
          </div>

          <mat-tab-group [selectedIndex]="selectedTabIndex()">
            <mat-tab label="Consultation">
              <div class="tab-body" [formGroup]="consultationGroup">
                <mat-form-field appearance="outline" class="full">
                  <mat-label>Chief Complaint</mat-label>
                  <textarea matInput rows="2" formControlName="chiefComplaint"></textarea>
                </mat-form-field>

                <mat-form-field appearance="outline" class="half">
                  <mat-label>Duration</mat-label>
                  <mat-select formControlName="duration">
                    <mat-option value="<1 week">&lt;1 week</mat-option>
                    <mat-option value="1-4 weeks">1-4 weeks</mat-option>
                    <mat-option value="1-3 months">1-3 months</mat-option>
                    <mat-option value=">3 months">&gt;3 months</mat-option>
                  </mat-select>
                </mat-form-field>

                <div class="half toggle-row">
                  <span>Expectation</span>
                  <mat-slide-toggle formControlName="expectationRealistic">Realistic</mat-slide-toggle>
                </div>

                <div class="full checkbox-grid">
                  <label>Medical conditions</label>
                  <div class="checks" [formGroup]="medicalConditionsGroup">
                    <mat-checkbox formControlName="diabetes">Diabetes</mat-checkbox>
                    <mat-checkbox formControlName="hypertension">Hypertension</mat-checkbox>
                    <mat-checkbox formControlName="keloid">Keloid tendency</mat-checkbox>
                    <mat-checkbox formControlName="autoimmune">Autoimmune disease</mat-checkbox>
                    <mat-checkbox formControlName="bleedingDisorder">Bleeding disorder</mat-checkbox>
                  </div>
                </div>

                <mat-form-field appearance="outline" class="half">
                  <mat-label>Medications</mat-label>
                  <mat-select formControlName="medications" multiple>
                    <mat-option value="anticoagulants">Anticoagulants</mat-option>
                    <mat-option value="retinoids">Retinoids</mat-option>
                    <mat-option value="steroids">Steroids</mat-option>
                    <mat-option value="immunosuppressants">Immunosuppressants</mat-option>
                  </mat-select>
                </mat-form-field>

                <mat-form-field appearance="outline" class="half">
                  <mat-label>Allergies</mat-label>
                  <mat-select formControlName="allergySelections" multiple>
                    <mat-option value="lidocaine">Lidocaine</mat-option>
                    <mat-option value="latex">Latex</mat-option>
                    <mat-option value="antibiotics">Antibiotics</mat-option>
                    <mat-option value="other">Other</mat-option>
                  </mat-select>
                </mat-form-field>

                <mat-form-field appearance="outline" class="full">
                  <mat-label>Allergies (free text)</mat-label>
                  <textarea matInput rows="2" formControlName="allergyNotes"></textarea>
                </mat-form-field>

                <div class="half toggle-row"><span>HSV history</span><mat-slide-toggle formControlName="hsvHistory"></mat-slide-toggle></div>
                <div class="half toggle-row"><span>Pregnancy</span><mat-slide-toggle formControlName="pregnancy"></mat-slide-toggle></div>

                <mat-form-field appearance="outline" class="half">
                  <mat-label>Fitzpatrick skin type</mat-label>
                  <mat-select formControlName="fitzpatrickSkinType">
                    @for (skin of [1,2,3,4,5,6]; track skin) {
                      <mat-option [value]="skin">Type {{ skin }}</mat-option>
                    }
                  </mat-select>
                </mat-form-field>

                <mat-form-field appearance="outline" class="half">
                  <mat-label>Acne severity</mat-label>
                  <mat-select formControlName="acneSeverity">
                    <mat-option value="mild">Mild</mat-option>
                    <mat-option value="moderate">Moderate</mat-option>
                    <mat-option value="severe">Severe</mat-option>
                  </mat-select>
                </mat-form-field>

                <mat-form-field appearance="outline" class="half">
                  <mat-label>Pigmentation</mat-label>
                  <mat-select formControlName="pigmentation">
                    <mat-option value="none">None</mat-option>
                    <mat-option value="mild">Mild</mat-option>
                    <mat-option value="moderate">Moderate</mat-option>
                    <mat-option value="severe">Severe</mat-option>
                  </mat-select>
                </mat-form-field>

                <mat-form-field appearance="outline" class="half">
                  <mat-label>Scarring (type + severity)</mat-label>
                  <input matInput formControlName="scarring" />
                </mat-form-field>

                <mat-form-field appearance="outline" class="half">
                  <mat-label>Volume loss</mat-label>
                  <mat-select formControlName="volumeLoss">
                    <mat-option value="mild">Mild</mat-option>
                    <mat-option value="moderate">Moderate</mat-option>
                    <mat-option value="severe">Severe</mat-option>
                  </mat-select>
                </mat-form-field>
              </div>
            </mat-tab>

            <mat-tab label="Neuromodulator">
              <div class="tab-body" [formGroup]="neuromodulatorGroup">
                <mat-form-field appearance="outline" class="half">
                  <mat-label>Product name</mat-label>
                  <mat-select formControlName="productName">
                    <mat-option value="Botox">Botox</mat-option>
                    <mat-option value="Dysport">Dysport</mat-option>
                    <mat-option value="Xeomin">Xeomin</mat-option>
                  </mat-select>
                </mat-form-field>
                <mat-form-field appearance="outline" class="half">
                  <mat-label>Lot number</mat-label>
                  <input matInput formControlName="lotNumber" />
                </mat-form-field>
                <mat-form-field appearance="outline" class="half">
                  <mat-label>Expiry date</mat-label>
                  <input matInput type="date" formControlName="expiryDate" />
                </mat-form-field>
                <mat-form-field appearance="outline" class="half">
                  <mat-label>Dilution</mat-label>
                  <input matInput type="number" formControlName="dilution" />
                </mat-form-field>
                <mat-form-field appearance="outline" class="half">
                  <mat-label>Total units drawn</mat-label>
                  <input matInput type="number" formControlName="totalUnitsDrawn" />
                </mat-form-field>

                <div class="full" [formGroup]="unitsAreaGroup">
                  <label class="block-title">Units injected per area</label>
                  <div class="grid-2">
                    <mat-form-field appearance="outline"><mat-label>Glabella</mat-label><input matInput type="number" formControlName="glabella" /></mat-form-field>
                    <mat-form-field appearance="outline"><mat-label>Forehead</mat-label><input matInput type="number" formControlName="forehead" /></mat-form-field>
                    <mat-form-field appearance="outline"><mat-label>Crow’s feet</mat-label><input matInput type="number" formControlName="crowsFeet" /></mat-form-field>
                    <mat-form-field appearance="outline"><mat-label>Masseter</mat-label><input matInput type="number" formControlName="masseter" /></mat-form-field>
                  </div>
                </div>

                <mat-form-field appearance="outline" class="half"><mat-label>Needle type</mat-label><input matInput formControlName="needleType" /></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Injection technique</mat-label><input matInput formControlName="injectionTechnique" /></mat-form-field>
                <div class="half toggle-row"><span>Complications</span><mat-slide-toggle formControlName="complications"></mat-slide-toggle></div>
                <mat-form-field appearance="outline" class="full"><mat-label>Post-care instructions</mat-label><textarea matInput rows="2" formControlName="postCareInstructions" readonly></textarea></mat-form-field>

                <div class="full">
                  <label class="block-title">Photos</label>
                  <div class="photo-toolbar">
                    <mat-form-field appearance="outline"><mat-label>Phase</mat-label><mat-select #nPhase><mat-option value="Before">Before</mat-option><mat-option value="After">After</mat-option></mat-select></mat-form-field>
                    <mat-form-field appearance="outline"><mat-label>Tag</mat-label><mat-select #nTag><mat-option value="frontal">frontal</mat-option><mat-option value="left">left</mat-option><mat-option value="right">right</mat-option><mat-option value="profile">profile</mat-option></mat-select></mat-form-field>
                    <input #nPhoto type="file" accept="image/*" (change)="onPhotoSelected('neuromodulator', nPhoto.files, nPhase.value || 'Before', nTag.value || 'frontal')" />
                  </div>
                  @for (item of tabPhotos().neuromodulator; track $index) {
                    <div class="photo-item">{{ item.phase }} · {{ item.tag }} · {{ item.fileName }}</div>
                  }
                  <div class="compare-grid">
                    <img *ngFor="let img of getComparisonImages('neuromodulator').before" [src]="img.url || ''" alt="before" />
                    <img *ngFor="let img of getComparisonImages('neuromodulator').after" [src]="img.url || ''" alt="after" />
                  </div>
                </div>
              </div>
            </mat-tab>

            <mat-tab label="Dermal Filler">
              <div class="tab-body" [formGroup]="dermalFillerGroup">
                <mat-form-field appearance="outline" class="half"><mat-label>Product name</mat-label><input matInput formControlName="productName" /></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Volume per syringe</mat-label><input matInput type="number" formControlName="volumePerSyringe" /></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Total volume used</mat-label><input matInput type="number" formControlName="totalVolumeUsed" /></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Injection areas</mat-label><mat-select formControlName="injectionAreas" multiple><mat-option value="lips">Lips</mat-option><mat-option value="cheeks">Cheeks</mat-option><mat-option value="nasolabial">Nasolabial folds</mat-option><mat-option value="jawline">Jawline</mat-option></mat-select></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Plane</mat-label><mat-select formControlName="plane"><mat-option value="subdermal">Subdermal</mat-option><mat-option value="supraperiosteal">Supraperiosteal</mat-option><mat-option value="deep-dermal">Deep dermal</mat-option></mat-select></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Cannula or needle</mat-label><mat-select formControlName="cannulaOrNeedle"><mat-option value="cannula">Cannula</mat-option><mat-option value="needle">Needle</mat-option></mat-select></mat-form-field>
                <div class="half toggle-row"><span>Aspiration performed</span><mat-slide-toggle formControlName="aspirationPerformed"></mat-slide-toggle></div>
                <mat-form-field appearance="outline" class="full"><mat-label>Immediate outcome</mat-label><textarea matInput rows="2" formControlName="immediateOutcome"></textarea></mat-form-field>
                <div class="full action-row"><button mat-raised-button color="warn" type="button" (click)="showVascularProtocol()">Vascular Occlusion Protocol</button></div>

                <div class="full">
                  <label class="block-title">Photos</label>
                  <div class="photo-toolbar">
                    <mat-form-field appearance="outline"><mat-label>Phase</mat-label><mat-select #dPhase><mat-option value="Before">Before</mat-option><mat-option value="After">After</mat-option></mat-select></mat-form-field>
                    <mat-form-field appearance="outline"><mat-label>Tag</mat-label><mat-select #dTag><mat-option value="frontal">frontal</mat-option><mat-option value="left">left</mat-option><mat-option value="right">right</mat-option><mat-option value="profile">profile</mat-option></mat-select></mat-form-field>
                    <input #dPhoto type="file" accept="image/*" (change)="onPhotoSelected('dermalFiller', dPhoto.files, dPhase.value || 'Before', dTag.value || 'frontal')" />
                  </div>
                  @for (item of tabPhotos().dermalFiller; track $index) {
                    <div class="photo-item">{{ item.phase }} · {{ item.tag }} · {{ item.fileName }}</div>
                  }
                  <div class="compare-grid">
                    <img *ngFor="let img of getComparisonImages('dermalFiller').before" [src]="img.url || ''" alt="before" />
                    <img *ngFor="let img of getComparisonImages('dermalFiller').after" [src]="img.url || ''" alt="after" />
                  </div>
                </div>
              </div>
            </mat-tab>

            <mat-tab label="Laser">
              <div class="tab-body" [formGroup]="laserGroup">
                <mat-form-field appearance="outline" class="half"><mat-label>Device name</mat-label><input matInput formControlName="deviceName" /></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Wavelength</mat-label><input matInput formControlName="wavelength" /></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Fluence</mat-label><input matInput formControlName="fluence" /></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Pulse duration</mat-label><input matInput formControlName="pulseDuration" /></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Spot size</mat-label><input matInput formControlName="spotSize" /></mat-form-field>
                <mat-form-field appearance="outline" class="half"><mat-label>Endpoint</mat-label><mat-select formControlName="endpoint"><mat-option value="erythema">Erythema</mat-option><mat-option value="edema">Edema</mat-option></mat-select></mat-form-field>
                <div class="half toggle-row"><span>Test patch</span><mat-slide-toggle formControlName="testPatch"></mat-slide-toggle></div>

                <div class="full">
                  <label class="block-title">Photos</label>
                  <div class="photo-toolbar">
                    <mat-form-field appearance="outline"><mat-label>Phase</mat-label><mat-select #lPhase><mat-option value="Before">Before</mat-option><mat-option value="After">After</mat-option></mat-select></mat-form-field>
                    <mat-form-field appearance="outline"><mat-label>Tag</mat-label><mat-select #lTag><mat-option value="frontal">frontal</mat-option><mat-option value="left">left</mat-option><mat-option value="right">right</mat-option><mat-option value="profile">profile</mat-option></mat-select></mat-form-field>
                    <input #lPhoto type="file" accept="image/*" (change)="onPhotoSelected('laser', lPhoto.files, lPhase.value || 'Before', lTag.value || 'frontal')" />
                  </div>
                  @for (item of tabPhotos().laser; track $index) {
                    <div class="photo-item">{{ item.phase }} · {{ item.tag }} · {{ item.fileName }}</div>
                  }
                  <div class="compare-grid">
                    <img *ngFor="let img of getComparisonImages('laser').before" [src]="img.url || ''" alt="before" />
                    <img *ngFor="let img of getComparisonImages('laser').after" [src]="img.url || ''" alt="after" />
                  </div>
                </div>
              </div>
            </mat-tab>
          </mat-tab-group>

          <div class="save-row">
            <button mat-raised-button color="primary" type="button" (click)="saveOrUpdate()" [disabled]="loadingIndicator">
              {{ currentConsultationId() ? 'Update All Tabs' : 'Save All Tabs' }}
            </button>
          </div>
        </form>
      </mat-card>
    </div>
  `,
  styles: [`
    .procedures-page { padding: 20px; }
    .page-header { margin-bottom: 16px; }
    .subtitle { color: #666; margin: 4px 0 0; }
    .form-shell { padding: 12px; }
    .patient-row { margin-bottom: 8px; }
    .patient-field { width: min(460px, 100%); }
    .tab-body { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 12px; padding: 14px 2px 2px; }
    .full { grid-column: 1 / -1; }
    .half { grid-column: span 1; }
    .toggle-row { display: flex; align-items: center; justify-content: space-between; padding: 8px 4px; }
    .checkbox-grid .checks { display: flex; flex-wrap: wrap; gap: 12px; margin-top: 8px; }
    .block-title { font-weight: 600; margin-bottom: 8px; display: inline-block; }
    .grid-2 { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 12px; }
    .photo-toolbar { display: grid; grid-template-columns: 170px 170px 1fr; gap: 10px; align-items: center; margin-bottom: 8px; }
    .photo-item { font-size: .9rem; padding: 4px 0; color: #444; }
    .compare-grid { display: grid; grid-template-columns: repeat(2, minmax(0, 220px)); gap: 10px; margin-top: 8px; }
    .compare-grid img { width: 100%; height: 160px; object-fit: cover; border: 1px solid #ddd; border-radius: 6px; }
    .action-row { margin-top: 4px; }
    .save-row { display: flex; justify-content: flex-end; margin-top: 16px; }
    @media (max-width: 992px) {
      .tab-body, .grid-2 { grid-template-columns: 1fr; }
      .photo-toolbar { grid-template-columns: 1fr; }
      .half { grid-column: 1 / -1; }
    }
  `]
})
export class ProceduresComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly endpoint = inject(AestheticEndpoint);
  private readonly alertService = inject(AlertService);
  private readonly route = inject(ActivatedRoute);

  loadingIndicator = false;
  readonly patients = signal<AestheticPatient[]>([]);
  readonly currentConsultationId = signal<number | null>(null);
  readonly selectedTabIndex = signal(0);

  readonly tabPhotos = signal<Record<PhotoTab, TabPhotoItem[]>>({
    neuromodulator: [],
    dermalFiller: [],
    laser: []
  });

  form = this.fb.nonNullable.group({
    patientId: [0, Validators.min(1)],
    consultation: this.fb.nonNullable.group({
      chiefComplaint: [''],
      duration: [''],
      expectationRealistic: [true],
      medicalConditions: this.fb.nonNullable.group({
        diabetes: [false],
        hypertension: [false],
        keloid: [false],
        autoimmune: [false],
        bleedingDisorder: [false]
      }),
      medications: [[] as string[]],
      allergySelections: [[] as string[]],
      allergyNotes: [''],
      hsvHistory: [false],
      pregnancy: [false],
      fitzpatrickSkinType: [1],
      acneSeverity: ['mild'],
      pigmentation: ['none'],
      scarring: [''],
      volumeLoss: ['mild']
    }),
    neuromodulator: this.fb.nonNullable.group({
      productName: ['Botox'],
      lotNumber: ['', Validators.required],
      expiryDate: ['', Validators.required],
      dilution: [0],
      totalUnitsDrawn: [0],
      unitsPerArea: this.fb.nonNullable.group({
        glabella: [0],
        forehead: [0],
        crowsFeet: [0],
        masseter: [0]
      }),
      needleType: [''],
      injectionTechnique: [''],
      complications: [false],
      postCareInstructions: ['']
    }),
    dermalFiller: this.fb.nonNullable.group({
      productName: [''],
      volumePerSyringe: [0],
      totalVolumeUsed: [0],
      injectionAreas: [[] as string[]],
      plane: ['subdermal'],
      cannulaOrNeedle: ['cannula'],
      aspirationPerformed: [false],
      immediateOutcome: ['']
    }),
    laser: this.fb.nonNullable.group({
      deviceName: [''],
      wavelength: [''],
      fluence: [''],
      pulseDuration: [''],
      spotSize: [''],
      endpoint: ['erythema'],
      testPatch: [false]
    })
  });

  get consultationGroup() { return this.form.controls.consultation; }
  get medicalConditionsGroup() { return this.form.controls.consultation.controls.medicalConditions; }
  get neuromodulatorGroup() { return this.form.controls.neuromodulator; }
  get unitsAreaGroup() { return this.form.controls.neuromodulator.controls.unitsPerArea; }
  get dermalFillerGroup() { return this.form.controls.dermalFiller; }
  get laserGroup() { return this.form.controls.laser; }

  readonly generatedPostCare = computed(() => {
    const n = this.neuromodulatorGroup.getRawValue();
    const units = this.unitsAreaGroup.getRawValue();
    const areaCount = Object.values(units).filter(x => Number(x) > 0).length;
    const advice = [
      'Remain upright for 4 hours.',
      'Avoid facial massage for 24 hours.',
      'No strenuous activity for 24 hours.'
    ];

    if (areaCount > 2) {
      advice.push('Use cool compress if mild swelling occurs.');
    }

    if (n.complications) {
      advice.push('Return immediately if pain, visual changes, or severe asymmetry occur.');
    }

    return advice.join(' ');
  });

  ngOnInit(): void {
    this.loadPatients();

    const initialTab = (this.route.snapshot.data?.['initialTab'] || '').toString();
    this.selectedTabIndex.set(this.mapTabToIndex(initialTab));

    this.neuromodulatorGroup.valueChanges.subscribe(() => {
      this.neuromodulatorGroup.controls.postCareInstructions.setValue(this.generatedPostCare(), { emitEvent: false });
    });
    this.neuromodulatorGroup.controls.postCareInstructions.setValue(this.generatedPostCare(), { emitEvent: false });
  }

  onPatientChanged(): void {
    const patientId = this.form.controls.patientId.value;
    const patient = this.patients().find(x => x.id === patientId);
    const existing = (patient?.consultations || [])
      .filter(c => (c.procedureType || '').toLowerCase() === 'procedures')
      .sort((a, b) => (b.consultationDate || '').localeCompare(a.consultationDate || ''))[0];

    this.loadFromConsultation(existing);
  }

  onPhotoSelected(tab: PhotoTab, files: FileList | null, phase: string, tag: string): void {
    const file = files?.item(0);
    if (!file) return;

    const safePhase: PhotoPhase = phase === 'After' ? 'After' : 'Before';
    const safeTag: StandardTag = this.toStandardTag(tag);

    this.tabPhotos.update(current => ({
      ...current,
      [tab]: [...current[tab], { file, fileName: file.name, phase: safePhase, tag: safeTag, url: URL.createObjectURL(file) }]
    }));
  }

  getComparisonImages(tab: PhotoTab): { before: TabPhotoItem[]; after: TabPhotoItem[] } {
    const items = this.tabPhotos()[tab];
    return {
      before: items.filter(x => x.phase === 'Before'),
      after: items.filter(x => x.phase === 'After')
    };
  }

  showVascularProtocol(): void {
    this.alertService.showStickyMessage(
      'Vascular Occlusion Protocol',
      'Stop injection immediately, massage area, apply warm compress, consider hyaluronidase protocol, and monitor perfusion urgently.',
      MessageSeverity.warn);
  }

  saveOrUpdate(): void {
    if (this.form.invalid) {
      this.alertService.showStickyMessage('Validation error', 'Please complete required fields.', MessageSeverity.warn);
      return;
    }

    if (!this.hasMandatoryBaselines()) {
      this.alertService.showStickyMessage('Baseline photos required', 'Baseline (Before) photos are mandatory for Neuromodulator, Dermal Filler, and Laser tabs.', MessageSeverity.warn);
      return;
    }

    const payload = this.buildPayload();
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage(this.currentConsultationId() ? 'Updating procedures...' : 'Saving procedures...');

    const consultationRequest = this.currentConsultationId()
      ? this.endpoint.updateConsultationEndpoint<AestheticConsultation>(this.currentConsultationId()!, payload)
      : this.endpoint.createConsultationEndpoint<AestheticConsultation>(payload);

    consultationRequest.subscribe({
      next: consultation => {
        this.currentConsultationId.set(consultation.id);
        this.uploadPendingPhotos(consultation.id);
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Save error', 'Unable to save procedures.', MessageSeverity.error, error);
      }
    });
  }

  private loadPatients(): void {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading patients...');

    this.endpoint.getPatientsEndpoint<AestheticPatient[]>().subscribe({
      next: patients => {
        this.patients.set(patients || []);
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
      },
      error: error => {
        this.loadingIndicator = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage('Load error', 'Unable to load patients.', MessageSeverity.error, error);
      }
    });
  }

  private loadFromConsultation(consultation?: AestheticConsultation): void {
    this.currentConsultationId.set(consultation?.id ?? null);

    if (!consultation) {
      this.form.controls.consultation.reset({
        chiefComplaint: '', duration: '', expectationRealistic: true,
        medicalConditions: { diabetes: false, hypertension: false, keloid: false, autoimmune: false, bleedingDisorder: false },
        medications: [], allergySelections: [], allergyNotes: '', hsvHistory: false, pregnancy: false,
        fitzpatrickSkinType: 1, acneSeverity: 'mild', pigmentation: 'none', scarring: '', volumeLoss: 'mild'
      });
      this.form.controls.neuromodulator.reset({
        productName: 'Botox', lotNumber: '', expiryDate: '', dilution: 0, totalUnitsDrawn: 0,
        unitsPerArea: { glabella: 0, forehead: 0, crowsFeet: 0, masseter: 0 }, needleType: '', injectionTechnique: '', complications: false,
        postCareInstructions: this.generatedPostCare()
      });
      this.form.controls.dermalFiller.reset({
        productName: '', volumePerSyringe: 0, totalVolumeUsed: 0, injectionAreas: [], plane: 'subdermal', cannulaOrNeedle: 'cannula', aspirationPerformed: false, immediateOutcome: ''
      });
      this.form.controls.laser.reset({ deviceName: '', wavelength: '', fluence: '', pulseDuration: '', spotSize: '', endpoint: 'erythema', testPatch: false });
      this.tabPhotos.set({ neuromodulator: [], dermalFiller: [], laser: [] });
      return;
    }

    const consultationData = this.tryParseJson(consultation.treatmentPlan);
    const neuromodulatorData = this.tryParseJson(consultation.injectionMapping);
    const dermalFillerData = this.tryParseJson(consultation.risksAndComplications);
    const laserData = this.tryParseJson(consultation.deviceSettings);

    this.form.controls.consultation.patchValue({
      chiefComplaint: consultation.procedureDescription || consultationData?.chiefComplaint || '',
      duration: consultationData?.duration || '',
      expectationRealistic: consultationData?.expectationRealistic ?? true,
      medicalConditions: consultationData?.medicalConditions || {},
      medications: consultationData?.medications || [],
      allergySelections: consultationData?.allergySelections || [],
      allergyNotes: consultationData?.allergyNotes || '',
      hsvHistory: consultationData?.hsvHistory ?? false,
      pregnancy: consultationData?.pregnancy ?? false,
      fitzpatrickSkinType: consultationData?.fitzpatrickSkinType ?? 1,
      acneSeverity: consultationData?.acneSeverity || 'mild',
      pigmentation: consultationData?.pigmentation || 'none',
      scarring: consultationData?.scarring || '',
      volumeLoss: consultationData?.volumeLoss || 'mild'
    });

    this.form.controls.neuromodulator.patchValue({
      productName: neuromodulatorData?.productName || 'Botox',
      lotNumber: consultation.lotNumber || neuromodulatorData?.lotNumber || '',
      expiryDate: neuromodulatorData?.expiryDate || '',
      dilution: Number(consultation.dilution ?? neuromodulatorData?.dilution ?? 0),
      totalUnitsDrawn: Number(neuromodulatorData?.totalUnitsDrawn ?? 0),
      unitsPerArea: neuromodulatorData?.unitsPerArea || { glabella: 0, forehead: 0, crowsFeet: 0, masseter: 0 },
      needleType: neuromodulatorData?.needleType || '',
      injectionTechnique: neuromodulatorData?.injectionTechnique || '',
      complications: neuromodulatorData?.complications ?? false,
      postCareInstructions: consultation.postTreatmentInstructions || this.generatedPostCare()
    });

    this.form.controls.dermalFiller.patchValue({
      productName: dermalFillerData?.productName || '',
      volumePerSyringe: Number(dermalFillerData?.volumePerSyringe ?? 0),
      totalVolumeUsed: Number(dermalFillerData?.totalVolumeUsed ?? 0),
      injectionAreas: dermalFillerData?.injectionAreas || [],
      plane: dermalFillerData?.plane || 'subdermal',
      cannulaOrNeedle: dermalFillerData?.cannulaOrNeedle || 'cannula',
      aspirationPerformed: dermalFillerData?.aspirationPerformed ?? false,
      immediateOutcome: dermalFillerData?.immediateOutcome || ''
    });

    this.form.controls.laser.patchValue({
      deviceName: consultation.deviceUsed || laserData?.deviceName || '',
      wavelength: consultation.wavelength || laserData?.wavelength || '',
      fluence: consultation.fluence || laserData?.fluence || '',
      pulseDuration: consultation.pulseDuration || laserData?.pulseDuration || '',
      spotSize: consultation.spotSize || laserData?.spotSize || '',
      endpoint: laserData?.endpoint || 'erythema',
      testPatch: laserData?.testPatch ?? false
    });

    this.tabPhotos.set(this.mapPhotosByTab(consultation.photos || []));
  }

  private buildPayload(): object {
    const consultation = this.consultationGroup.getRawValue();
    const neuromodulator = this.neuromodulatorGroup.getRawValue();
    const dermalFiller = this.dermalFillerGroup.getRawValue();
    const laser = this.laserGroup.getRawValue();

    return {
      id: this.currentConsultationId() ?? 0,
      patientId: this.form.controls.patientId.value,
      consultationDate: new Date().toISOString(),
      procedureType: 'Procedures',
      procedureDescription: consultation.chiefComplaint,
      treatmentPlan: JSON.stringify(consultation),
      injectionMapping: JSON.stringify(neuromodulator),
      risksAndComplications: JSON.stringify(dermalFiller),
      deviceSettings: JSON.stringify(laser),
      lotNumber: neuromodulator.lotNumber,
      dilution: neuromodulator.dilution?.toString() ?? '',
      postTreatmentInstructions: this.generatedPostCare(),
      deviceUsed: laser.deviceName,
      wavelength: laser.wavelength,
      fluence: laser.fluence,
      pulseDuration: laser.pulseDuration,
      spotSize: laser.spotSize
    };
  }

  private uploadPendingPhotos(consultationId: number): void {
    const items = [
      ...this.tabPhotos().neuromodulator.map(x => ({ tab: 'neuromodulator' as const, item: x })),
      ...this.tabPhotos().dermalFiller.map(x => ({ tab: 'dermalFiller' as const, item: x })),
      ...this.tabPhotos().laser.map(x => ({ tab: 'laser' as const, item: x }))
    ].filter(x => !!x.item.file);

    if (items.length === 0) {
      this.finishSaveSuccess();
      return;
    }

    let completed = 0;
    let failed = 0;

    for (const entry of items) {
      const formData = new FormData();
      formData.append('consultationId', String(consultationId));
      formData.append('type', `${entry.tab}|${entry.item.phase}|${entry.item.tag}`);
      formData.append('file', entry.item.file!, entry.item.fileName);

      this.endpoint.uploadPhotoEndpoint<AestheticPhoto>(formData).subscribe({
        next: () => {
          completed += 1;
          if (completed + failed === items.length) {
            this.finishSaveSuccess(failed);
          }
        },
        error: () => {
          failed += 1;
          if (completed + failed === items.length) {
            this.finishSaveSuccess(failed);
          }
        }
      });
    }
  }

  private finishSaveSuccess(failedUploads = 0): void {
    this.loadingIndicator = false;
    this.alertService.stopLoadingMessage();
    const message = failedUploads > 0
      ? `Procedures saved. ${failedUploads} photo upload(s) failed.`
      : 'Procedures saved successfully.';

    this.alertService.showMessage('Success', message, failedUploads > 0 ? MessageSeverity.warn : MessageSeverity.success);

    const patientId = this.form.controls.patientId.value;
    this.loadPatients();
    this.form.controls.patientId.setValue(patientId);
    this.onPatientChanged();
  }

  private hasMandatoryBaselines(): boolean {
    const photos = this.tabPhotos();
    return (['neuromodulator', 'dermalFiller', 'laser'] as PhotoTab[])
      .every(tab => photos[tab].some(x => x.phase === 'Before'));
  }

  private mapPhotosByTab(photos: AestheticPhoto[]): Record<PhotoTab, TabPhotoItem[]> {
    const mapped: Record<PhotoTab, TabPhotoItem[]> = {
      neuromodulator: [],
      dermalFiller: [],
      laser: []
    };

    for (const photo of photos) {
      const [tabRaw, phaseRaw, tagRaw] = (photo.type || '').split('|');
      if (!tabRaw || !phaseRaw || !tagRaw) continue;

      const tab = this.toPhotoTab(tabRaw);
      if (!tab) continue;

      mapped[tab].push({
        id: photo.id,
        consultationId: photo.consultationId,
        fileName: photo.fileName || 'photo',
        phase: phaseRaw === 'After' ? 'After' : 'Before',
        tag: this.toStandardTag(tagRaw),
        url: photo.url
      });
    }

    return mapped;
  }

  private toPhotoTab(raw: string): PhotoTab | null {
    if (raw === 'neuromodulator' || raw === 'dermalFiller' || raw === 'laser') {
      return raw;
    }

    return null;
  }

  private toStandardTag(raw: string): StandardTag {
    const normalized = raw.toLowerCase();
    if (normalized === 'left' || normalized === 'right' || normalized === 'profile') {
      return normalized;
    }

    return 'frontal';
  }

  private mapTabToIndex(tab: string): number {
    const normalized = tab.toLowerCase();
    if (normalized === 'neuromodulator' || normalized === 'botox') return 1;
    if (normalized === 'dermalfiller') return 2;
    if (normalized === 'laser') return 3;
    return 0;
  }

  private tryParseJson(value?: string): any {
    if (!value) return null;
    try {
      return JSON.parse(value);
    } catch {
      return null;
    }
  }
}
