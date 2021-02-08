using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace UtcTickTime.Behaviors
{
    public class RealtimeUpdateSourceBehavior : Behavior<TextBox>
    {
        public bool IsRealtimeUpdateSource
        {
            get => (bool)GetValue(IsRealtimeUpdateSourceProperty);
            set => SetValue(IsRealtimeUpdateSourceProperty, value);
        }

        public static readonly DependencyProperty IsRealtimeUpdateSourceProperty =
            DependencyProperty.Register(
                "IsRealtimeUpdateSource",
                typeof(bool),
                typeof(RealtimeUpdateSourceBehavior),
                new PropertyMetadata(false));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.TextChanged += TextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.TextChanged -= TextChanged;
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;
            if (!IsRealtimeUpdateSource) return;
            
            var binding = textBox.GetBindingExpression(TextBox.TextProperty);
            binding?.UpdateSource();
        }
    }
}