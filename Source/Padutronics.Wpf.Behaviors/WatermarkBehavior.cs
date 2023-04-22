using Microsoft.Xaml.Behaviors;
using Padutronics.Wpf.Behaviors.Adorners;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Padutronics.Wpf.Behaviors;

public abstract class WatermarkBehavior<TControl> : Behavior<TControl>
    where TControl : Control
{
    public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(
        nameof(Watermark),
        typeof(object),
        typeof(WatermarkBehavior<TControl>)
    );

    protected abstract bool ShouldShowWatermark { get; }

    public object Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
    {
        if (ShouldShowWatermark)
        {
            ShowWatermark();
        }
    }

    protected void HideWatermark()
    {
        if (TryGetAdornerLayer(out AdornerLayer adornerLayer))
        {
            Adorner[] adorners = adornerLayer.GetAdorners(AssociatedObject);
            if (adorners is not null)
            {
                foreach (Adorner adorner in adorners)
                {
                    if (adorner is WatermarkAdorner)
                    {
                        adorner.Visibility = Visibility.Hidden;

                        adornerLayer.Remove(adorner);
                    }
                }
            }
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.Loaded += AssociatedObject_Loaded;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.Loaded -= AssociatedObject_Loaded;

        base.OnDetaching();
    }

    protected void ShowWatermark()
    {
        if (TryGetAdornerLayer(out AdornerLayer adornerLayer))
        {
            adornerLayer.Add(new WatermarkAdorner(AssociatedObject, Watermark));
        }
    }

    private bool TryGetAdornerLayer(out AdornerLayer adornerLayer)
    {
        adornerLayer = AdornerLayer.GetAdornerLayer(AssociatedObject);

        return adornerLayer is not null;
    }
}