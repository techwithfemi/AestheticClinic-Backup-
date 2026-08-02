import { Pipe, PipeTransform } from '@angular/core';
import { formatUtcDateDashForDisplay, formatUtcDateForDisplay, formatUtcForDisplay, formatUtcTimeForDisplay } from '../shared/utils/utc-date.util';

@Pipe({
  name: 'utcDisplay',
  standalone: true
})
export class UtcDisplayPipe implements PipeTransform {
  transform(value?: string | Date, mode: 'datetime' | 'date' | 'dateDash' | 'time' = 'datetime'): string {
    switch (mode) {
      case 'date':
        return formatUtcDateForDisplay(value);
      case 'dateDash':
        return formatUtcDateDashForDisplay(value);
      case 'time':
        return formatUtcTimeForDisplay(value);
      default:
        return formatUtcForDisplay(value);
    }
  }
}
