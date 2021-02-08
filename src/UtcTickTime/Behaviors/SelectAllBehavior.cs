using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace UtcTickTime.Behaviors
{
    public class SelectAllBehavior : Behavior<TextBox>
    {
        public bool EnableAutoSelectAll
        {
            get => (bool)GetValue(EnableAutoSelectAllProperty);
            set => SetValue(EnableAutoSelectAllProperty, value);
        }

        public static readonly DependencyProperty EnableAutoSelectAllProperty =
            DependencyProperty.Register(
                "EnableAutoSelectAll",
                typeof(bool),
                typeof(SelectAllBehavior),
                new PropertyMetadata(false));
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.GotFocus += GotFocus;
            AssociatedObject.PreviewMouseLeftButtonDown += PreviewMouseLeftButtonDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.GotFocus -= GotFocus;
            AssociatedObject.PreviewMouseLeftButtonDown -= PreviewMouseLeftButtonDown;
        }
        private void GotFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;
            if (EnableAutoSelectAll) textBox.SelectAll();
        }

        private void PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;
            if (textBox.IsFocused) return;
            textBox.Focus();
            e.Handled = true;
        }
    }
}