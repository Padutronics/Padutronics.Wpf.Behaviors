using System.Windows.Controls;
using System.Windows.Input;

namespace Padutronics.Wpf.Behaviors;

public sealed class TextBoxWatermarkBehavior : WatermarkBehavior<TextBox>
{
    protected override bool ShouldShowWatermark => AssociatedObject.Text == string.Empty;

    private void AssociatedObject_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (ShouldShowWatermark)
        {
            HideWatermark();
        }
    }

    private void AssociatedObject_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (ShouldShowWatermark)
        {
            ShowWatermark();
        }
    }

    private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!ShouldShowWatermark)
        {
            HideWatermark();
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.GotKeyboardFocus += AssociatedObject_GotKeyboardFocus;
        AssociatedObject.LostKeyboardFocus += AssociatedObject_LostKeyboardFocus;
        AssociatedObject.TextChanged += AssociatedObject_TextChanged;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.GotKeyboardFocus -= AssociatedObject_GotKeyboardFocus;
        AssociatedObject.LostKeyboardFocus -= AssociatedObject_LostKeyboardFocus;
        AssociatedObject.TextChanged -= AssociatedObject_TextChanged;

        base.OnDetaching();
    }
}