$file = 'C:\Users\Administrator\source\repos\Medicals\AestheticClinic\AestheticEMR\AestheticEMR.client\src\app\features\aesthetics\procedures\procedures-entry-dialog.component.ts'
$content = Get-Content $file -Raw

# First, update the setValue to add emitEvent: false
$content = $content -replace 'this\.form\.controls\.patientId\.setValue\(selected\?\.patientId \?\? 0\);', 'this.form.controls.patientId.setValue(selected?.patientId ?? 0, { emitEvent: false });'

# Replace the line that sets selectedVisitPNo
$oldBlock = @"
    const selected = this.patientAttendanceOptions().find(x => x.consultId === normalizedConsultId);
    this.form.controls.patientId.setValue(selected?.patientId ?? 0, { emitEvent: false });
    this.selectedVisitPNo.set(selected?.pNo ?? '');
    this.selectedClinic.set('Aesthetic');
"@

$newBlock = @"
    const selected = this.patientAttendanceOptions().find(x => x.consultId === normalizedConsultId);
    this.form.controls.patientId.setValue(selected?.patientId ?? 0, { emitEvent: false });

    // Only update pNo if we found a matching option, otherwise preserve existing value
    if (selected) {
      this.selectedVisitPNo.set(selected.pNo ?? '');
    } else if (!normalizedConsultId) {
      this.selectedVisitPNo.set('');
    }
    // If consultId is set but no match found (data still loading), preserve existing pNo

    this.selectedClinic.set('Aesthetic');
"@

$content = $content -replace [regex]::Escape($oldBlock), $newBlock

Set-Content $file $content
Write-Host "File updated successfully"
