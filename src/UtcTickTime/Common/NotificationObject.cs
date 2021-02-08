using System.Reactive.Disposables;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UtcTickTime.Common
{
    public class NotificationObject : INotifyPropertyChanged, IDisposable
    {
        protected CompositeDisposable Disposables { get; } = new();

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T store, T value, [CallerMemberName] string propertyName = null)
        {
            if (!object.Equals(store, value)) return false;
            store = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
        #endregion

        #region IDisposable
        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                var handler = PropertyChanged;
                if (handler != null)
                {
                    foreach(PropertyChangedEventHandler h in handler.GetInvocationList())
                        PropertyChanged -= h;
                }

                Disposables.Dispose();
            }

            disposed = true;
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~NotificationObject()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose() => Dispose();
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}