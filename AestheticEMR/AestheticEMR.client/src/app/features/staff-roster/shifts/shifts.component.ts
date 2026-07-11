import { Component, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { MatCardModule } from '@angular/material/card';
import { fadeInOut } from '../../../services/animations';

@Component({
  selector: 'app-shifts',
  standalone: true,
  imports: [TranslateModule, MatCardModule],
  templateUrl: './shifts.component.html',
  styleUrls: ['./shifts.component.scss'],
  animations: [fadeInOut]
})
export class ShiftsComponent implements OnInit {
  ngOnInit(): void {}
}
