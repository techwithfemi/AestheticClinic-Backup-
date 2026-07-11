import { Component, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { MatCardModule } from '@angular/material/card';
import { fadeInOut } from '../../../services/animations';

@Component({
  selector: 'app-create-roster',
  standalone: true,
  imports: [TranslateModule, MatCardModule],
  templateUrl: './create-roster.component.html',
  styleUrls: ['./create-roster.component.scss'],
  animations: [fadeInOut]
})
export class CreateRosterComponent implements OnInit {
  ngOnInit(): void {}
}
