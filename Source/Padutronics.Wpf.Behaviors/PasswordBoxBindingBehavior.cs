using Microsoft.Xaml.Behaviors;
using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace Padutronics.Wpf.Behaviors;

public sealed class PasswordBoxBindingBehavior : Behavior<PasswordBox>
{
    public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
        nameof(Password),
        typeof(SecureString),
        typeof(PasswordBoxBindingBehavior),
        new FrameworkPropertyMetadata(OnPasswordChanged)
    );
    public static readonly DependencyProperty UpdateTriggerProperty = DependencyProperty.Register(
        nameof(UpdateTrigger),
        typeof(PasswordBoxBindingUpdateTrigger),
        typeof(PasswordBoxBindingBehavior),
        new PropertyMetadata(defaultValue: PasswordBoxBindingUpdateTrigger.LostFocus, OnUpdateTriggerChanged)
    );

    public SecureString Password
    {
        get => (SecureString)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }

    public PasswordBoxBindingUpdateTrigger UpdateTrigger
    {
        get => (PasswordBoxBindingUpdateTrigger)GetValue(UpdateTriggerProperty);
        set => SetValue(UpdateTriggerProperty, value);
    }

    private void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
    {
        UpdatePassword();
    }

    private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
    {
        UpdatePassword();
    }

    private void AttachEventHandlers()
    {
        switch (UpdateTrigger)
        {
            case PasswordBoxBindingUpdateTrigger.LostFocus:
                AssociatedObject.LostFocus += AssociatedObject_LostFocus;
                break;
            case PasswordBoxBindingUpdateTrigger.PropertyChanged:
                AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
                break;
        }
    }

    private void DetachEventHandlers()
    {
        AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
        AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        AttachEventHandlers();
    }

    protected override void OnDetaching()
    {
        DetachEventHandlers();

        base.OnDetaching();
    }

    private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behavior = (PasswordBoxBindingBehavior)d;
        if (behavior.AssociatedObject is not null)
        {
            var newPassword = (SecureString)e.NewValue;
            if (newPassword?.Length == 0)
            {
                behavior.AssociatedObject.Password = string.Empty;
            }
        }
    }

    private static void OnUpdateTriggerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behavior = (PasswordBoxBindingBehavior)d;
        if (behavior.AssociatedObject is not null)
        {
            behavior.DetachEventHandlers();
            behavior.AttachEventHandlers();
        }
    }

    private void UpdatePassword()
    {
        Password = AssociatedObject.SecurePassword;
    }
}