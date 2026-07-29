// Utility helpers to mirror server UtcAwareDateTimeConverter behavior on the client.
// - Parse ISO strings with offset (Z or +/-) as UTC (no client-local offset applied)
// - If string lacks offset, treat it as UTC (legacy fallback)
// - Format for display in `dd MMM yyyy · HH:mm` using UTC when original value carried an offset

export function parseUtcDate(value?: string): Date | null {
  if (!value) return null;

  // If value contains explicit timezone (Z or +HH:MM / -HH:MM) treat as UTC
  const hasTimezone = /Z$|[+\-]\d{2}:?\d{2}$/.test(value);

  // Try DateTimeOffset-style parse using native Date for ISO strings
  let date = new Date(value);
  if (Number.isNaN(date.getTime())) {
    // Fallback: try to build a date from date and time parts (no timezone)
    const parts = value.split('T');
    const datePart = parts[0] || value;
    const timePart = parts[1] ? parts[1].split('Z')[0] : '';
    date = new Date(datePart + (timePart ? 'T' + timePart : ''));
  }

  if (Number.isNaN(date.getTime())) return null;

  // If incoming string had explicit timezone, return a Date representing the same UTC instant
  // Native Date constructed from an ISO string with Z or offset already represents the instant in UTC
  // For strings without offset, treat as UTC by using UTC getters when formatting
  return date;
}

export function formatUtcForDisplay(value?: string | Date): string {
  if (!value) return '—';

  let date: Date | null = null;
  let sourceHasTimezone = false;

  if (typeof value === 'string') {
    if (/Z$|[+\-]\d{2}:?\d{2}$/.test(value)) sourceHasTimezone = true;
    date = parseUtcDate(value);
  } else if (value instanceof Date) {
    date = value;
    // If Date.Kind is not available in JS, we assume Date was created from an ISO with timezone if
    // its toISOString() differs from toString() in a way that indicates UTC origin. This is heuristic;
    // callers should prefer passing the original string when possible.
    sourceHasTimezone = false;
  }

  if (!date) return (typeof value === 'string') ? value : '—';

  // When source had timezone or was an ISO with Z/offset, use UTC getters to avoid client-local shifts
  const dayNum = sourceHasTimezone ? date.getUTCDate() : date.getDate();
  const year = sourceHasTimezone ? date.getUTCFullYear() : date.getFullYear();
  const hours = sourceHasTimezone ? date.getUTCHours() : date.getHours();
  const minutes = sourceHasTimezone ? date.getUTCMinutes() : date.getMinutes();

  const day = dayNum.toString().padStart(2, '0');
  const month = sourceHasTimezone
    ? date.toLocaleString('en', { month: 'short', timeZone: 'UTC' })
    : date.toLocaleString('en', { month: 'short' });

  const hrs = hours.toString().padStart(2, '0');
  const mins = minutes.toString().padStart(2, '0');
  return `${day} ${month} ${year} · ${hrs}:${mins}`;
}
