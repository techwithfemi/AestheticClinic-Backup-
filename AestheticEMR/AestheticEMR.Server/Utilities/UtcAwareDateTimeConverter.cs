// ---------------------------------------
// Custom System.Text.Json converters for DateTime / DateTime?.
// Goal: ensure every DateTime on the wire carries an explicit offset so the
// client (Angular `Date.parse`) never has to guess between UTC and local.
//
// Behavior:
//   Serialize: the value is converted to the server's local timezone and emitted
//              with a numeric offset (e.g. "2026-07-27T09:30:00+01:00"). This
//              matches what the user sees on the server clock and on the wall
//              clock the clinic operates on.
//   Deserialize: any incoming string with or without offset is converted to the
//                server's local timezone before being stored. Values lacking an
//                offset are treated as UTC (the historical convention for any
//                values that were written before this converter was in place).
// ---------------------------------------

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AestheticEMR.Server.Serialization
{
    public class UtcAwareDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var raw = reader.GetString();
            if (string.IsNullOrWhiteSpace(raw))
            {
                return DateTime.MinValue;
            }

            // Parse with offset awareness. DateTimeOffset handles all three forms:
            //   "...Z"     -> UTC
            //   "...+HH:MM" -> that offset
            //   "..." (no offset) -> treated as local (matches ECMAScript); we
            //                         then reinterpret as UTC for parity with
            //                         legacy data that was originally UTC.
            if (DateTimeOffset.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var dto))
            {
                return dto.LocalDateTime;
            }

            if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var dt))
            {
                return DateTime.SpecifyKind(dt, DateTimeKind.Local);
            }

            return DateTime.MinValue;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            var local = value.Kind switch
            {
                DateTimeKind.Utc => value.ToLocalTime(),
                DateTimeKind.Local => value,
                _ => DateTime.SpecifyKind(value, DateTimeKind.Utc).ToLocalTime()
            };

            writer.WriteStringValue(local.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz", CultureInfo.InvariantCulture));
        }
    }

    public class UtcAwareNullableDateTimeConverter : JsonConverter<DateTime?>
    {
        private static readonly UtcAwareDateTimeConverter Inner = new();

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            var raw = reader.GetString();
            if (string.IsNullOrWhiteSpace(raw))
            {
                return null;
            }

            if (DateTimeOffset.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var dto))
            {
                return dto.LocalDateTime;
            }

            if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var dt))
            {
                return DateTime.SpecifyKind(dt, DateTimeKind.Local);
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (!value.HasValue)
            {
                writer.WriteNullValue();
                return;
            }

            Inner.Write(writer, value.Value, options);
        }
    }
}