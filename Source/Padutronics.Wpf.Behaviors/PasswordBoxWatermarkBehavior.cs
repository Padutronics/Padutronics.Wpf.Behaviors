using System.Windows.Controls;
using System.Windows.Input;

namespace Padutronics.Wpf.Behaviors;

public sealed class PasswordBoxWatermarkBehavior : WatermarkBehavior<PasswordBox>
{
    protected override bool ShouldShowWatermark => AssociatedObject.Password == string.Empty;

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

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.GotKeyboardFocus += AssociatedObject_GotKeyboardFocus;
        AssociatedObject.LostKeyboardFocus += AssociatedObject_LostKeyboardFocus;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.GotKeyboardFocus -= AssociatedObject_GotKeyboardFocus;
        AssociatedObject.LostKeyboardFocus -= AssociatedObject_LostKeyboardFocus;

        base.OnDetaching();
    }
}