// Utility helpers to mirror server UtcAwareDateTimeConverter behavior on the client.
// - Parse ISO strings with offset (Z or +/-) as UTC (no client-local offset applied)
// - If string lacks offset, treat it as UTC (legacy fallback)
// - Format for display in `dd MMM yyyy · HH:mm` using UTC to prevent client-local shifts

function hasTimezoneOffset(value: string): boolean {
  return /Z$|[+-]\d{2}:?\d{2}$/.test(value);
}

export function parseUtcDate(value?: string): Date | null {
  if (!value) return null;

  const hasTimezone = hasTimezoneOffset(value);
  const normalized = hasTimezone ? value : `${value}Z`;

  // Parse as UTC instant (explicit offset if present, otherwise legacy fallback as UTC)
  let date = new Date(normalized);
  if (Number.isNaN(date.getTime())) {
    // Fallback: try to build a date from date and time parts
    const parts = value.split('T');
    const datePart = parts[0] || value;
    const timePart = parts[1] ? parts[1].split('Z')[0] : '';
    const rebuilt = `${datePart}${timePart ? `T${timePart}` : ''}${hasTimezone ? '' : 'Z'}`;
    date = new Date(rebuilt);
  }

  if (Number.isNaN(date.getTime())) return null;
  return date;
}

export function formatUtcForDisplay(value?: string | Date): string {
  if (!value) return '—';

  let date: Date | null = null;
  let useUtcGetters = false;

  if (typeof value === 'string') {
    date = parseUtcDate(value);
    // Mirror UtcAwareDateTimeConverter: all string DateTime payloads are interpreted as UTC
    useUtcGetters = true;
  } else if (value instanceof Date) {
    date = value;
    useUtcGetters = false;
  }

  if (!date) return (typeof value === 'string') ? value : '—';

  const dayNum = useUtcGetters ? date.getUTCDate() : date.getDate();
  const year = useUtcGetters ? date.getUTCFullYear() : date.getFullYear();
  const hours = useUtcGetters ? date.getUTCHours() : date.getHours();
  const minutes = useUtcGetters ? date.getUTCMinutes() : date.getMinutes();

  const day = dayNum.toString().padStart(2, '0');
  const month = useUtcGetters
    ? date.toLocaleString('en', { month: 'short', timeZone: 'UTC' })
    : date.toLocaleString('en', { month: 'short' });

  const hrs = hours.toString().padStart(2, '0');
  const mins = minutes.toString().padStart(2, '0');
  return `${day} ${month} ${year} · ${hrs}:${mins}`;
}

export function formatUtcDateForDisplay(value?: string | Date): string {
  const formatted = formatUtcForDisplay(value);
  if (formatted === '—') return '—';
  const [datePart] = formatted.split(' · ');
  return datePart || '—';
}

export function formatUtcDateDashForDisplay(value?: string | Date): string {
  const datePart = formatUtcDateForDisplay(value);
  return datePart === '—' ? '—' : datePart.replace(/ /g, '-');
}

export function formatUtcTimeForDisplay(value?: string | Date): string {
  const formatted = formatUtcForDisplay(value);
  if (formatted === '—') return '—';
  const [, timePart] = formatted.split(' · ');
  return timePart || '—';
}
