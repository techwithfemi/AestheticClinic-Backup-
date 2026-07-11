import { Component, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { MatCardModule } from '@angular/material/card';
import { fadeInOut } from '../../../services/animations';

@Component({
  selector: 'app-staff-group',
  standalone: true,
  imports: [TranslateModule, MatCardModule],
  templateUrl: './staff-group.component.html',
  styleUrls: ['./staff-group.component.scss'],
  animations: [fadeInOut]
})
export class StaffGroupComponent implements OnInit {
  ngOnInit(): void {}
}
