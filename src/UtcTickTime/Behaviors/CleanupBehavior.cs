using System;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace UtcTickTime.Behaviors
{
    public class CleanupBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Closed += Closed;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Closed -= Closed;
        }

        private void Closed(object sender, EventArgs e)
        {
            if (AssociatedObject.DataContext is IDisposable disposable)
                disposable.Dispose();
        }
    }
}