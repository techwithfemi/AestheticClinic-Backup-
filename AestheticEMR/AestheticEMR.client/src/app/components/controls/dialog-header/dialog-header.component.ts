import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-dialog-header',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule],
  templateUrl: './dialog-header.component.html',
  styleUrl: './dialog-header.component.scss'
})
export class DialogHeaderComponent {
  @Input() title = '';
  @Input() icon = 'assignment_ind';
  @Input() closeAriaLabel = 'Close dialog';
  @Output() closeClicked = new EventEmitter<void>();

  onClose(): void {
    this.closeClicked.emit();
  }
}
