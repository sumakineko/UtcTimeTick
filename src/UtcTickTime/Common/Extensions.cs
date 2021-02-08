using System;

namespace UtcTickTime.Common
{
    public static class Extensions
    {
        public static DateTime? Truncate(this DateTime? self, DateTimeTruncateLevels level)
        {
            DateTime? TrancateCore(TimeSpan span) => self.Value.AddTicks(-(self.Value.Ticks % span.Ticks));

            if (!self.HasValue) return self;

            return level switch {
                DateTimeTruncateLevels.Second => TrancateCore(TimeSpan.FromSeconds(1)),
                DateTimeTruncateLevels.Minute => TrancateCore(TimeSpan.FromMinutes(1)),
                DateTimeTruncateLevels.Hour => TrancateCore(TimeSpan.FromHours(1)),
                DateTimeTruncateLevels.Day => TrancateCore(TimeSpan.FromDays(1)),
                _ => self,
            };
        }

        public static bool TryParseDateTime(this string self, out DateTime value) => DateTime.TryParse(self, out value);
        public static string PadLeft(this long self, int totalWidth, char paddingChar) => self.ToString().PadLeft(totalWidth, paddingChar);
        public static string Format(this string self, Func<string, string> formatter) => formatter?.Invoke(self) ?? string.Empty;
    }
}