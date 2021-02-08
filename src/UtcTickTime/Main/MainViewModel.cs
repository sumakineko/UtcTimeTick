using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using UtcTickTime.Common;

namespace UtcTickTime.Main
{
    public class MainViewModel : NotificationObject
    {
        public MainViewModel()
        {
            TruncateLevels = new ObservableCollection<DateTimeTruncateLevels>(Enum.GetValues(typeof(DateTimeTruncateLevels)).Cast<DateTimeTruncateLevels>()).ToReadOnlyReactiveCollection(x => x);
            SelectedTruncateLevel = new ReactiveProperty<DateTimeTruncateLevels>(DateTimeTruncateLevels.Minute).AddTo(Disposables);
            Date = new ReactiveProperty<DateTime?>(DateTime.Now);
            DisplayDate = Date.Select(x => x.Truncate(SelectedTruncateLevel.Value))
                              .ToReactiveProperty()
                              .AddTo(Disposables);
            NowCommand = new();
            NowCommand.Subscribe(_ => Date.Value = DateTime.Now)
                      .AddTo(Disposables);

            Now = Date.Where(x => x.HasValue)
                      .Select(x => x?.ToString(FORMAT))
                      .ToReactiveProperty()
                      .AddTo(Disposables);

            Now.SetValidateNotifyError(x => DateTime.TryParse(x, out var d) ? null : "日付に変換できません。")
                .Subscribe(x =>
                {
                    if (!x.TryParseDateTime(out var d))
                    {
                        Date.Value = null;
                        return;
                    }
                    Date.Value = d;
                })
                .AddTo(Disposables);

            Jst = DisplayDate.Select(x => x.HasValue ? x.Value.ToString(FORMAT) : string.Empty)
                             .ToReactiveProperty()
                             .AddTo(Disposables);

            SelectedTruncateLevel.Subscribe(x => DisplayDate.Value = Date.Value.Truncate(x))
                                 .AddTo(Disposables);

            Utc = DisplayDate
                .Select(x =>
                {
                    if (!x.HasValue) return string.Empty;

                    return x.Value.ToString(FORMAT).Format(y =>
                    {
                        var regex = new Regex(@"(?<date>\d{4}-\d{2}-\d{2}) (?<time>\d{2}:\d{2}:\d{2}\.\d+)");
                        var match = regex.Match(y);
                        return match.Success ? $"{match.Groups["date"].Value}T{match.Groups["time"].Value}Z" : string.Empty;
                    });
                })
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposables);

            QueryString = Utc.Select(x => string.IsNullOrEmpty(x) ? x : $"datetime'{x}'")
                            .ToReadOnlyReactiveProperty()
                             .AddTo(Disposables);

            Ticks = DisplayDate.Select(x => x.HasValue ? x.Value.ToUniversalTime().Ticks.ToString().PadLeft(19, '0') : string.Empty)
                               .ToReadOnlyReactiveProperty()
                               .AddTo(Disposables);
        }

        private const string FORMAT = "yyyy-MM-dd HH:mm:ss.fff";

        private ReactiveProperty<DateTime?> Date { get; set; }
        private ReactiveProperty<DateTime?> DisplayDate { get; set; }

        public ReactiveProperty<DateTimeTruncateLevels> SelectedTruncateLevel { get; private set; }
        public ReadOnlyObservableCollection<DateTimeTruncateLevels> TruncateLevels { get; private set; }

        public ReactiveProperty<string> Now { get; private set; }
        public ReactiveProperty<string> Jst { get; private set; }
        public ReadOnlyReactiveProperty<string> Utc { get; private set; }
        public ReadOnlyReactiveProperty<string> Ticks { get; private set; }
        public ReadOnlyReactiveProperty<string> QueryString { get; private set; }
        public ReactiveCommand NowCommand { get; set; }
    }
}