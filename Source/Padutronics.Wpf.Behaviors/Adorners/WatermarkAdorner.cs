using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace Padutronics.Wpf.Behaviors.Adorners;

internal sealed class WatermarkAdorner : Adorner
{
    private readonly ContentPresenter contentPresenter;

    public WatermarkAdorner(Control adornedElement, object watermark) :
        base(adornedElement)
    {
        IsHitTestVisible = false;

        contentPresenter = new ContentPresenter
        {
            Content = watermark,
            Margin = Control.Padding
        };

        var binding = new Binding(nameof(IsVisible))
        {
            Converter = new BooleanToVisibilityConverter(),
            Source = adornedElement
        };

        SetBinding(VisibilityProperty, binding);
    }

    private Control Control => (Control)AdornedElement;

    protected override int VisualChildrenCount => 1;

    protected override Size ArrangeOverride(Size finalSize)
    {
        contentPresenter.Arrange(new Rect(finalSize));

        return finalSize;
    }

    protected override Visual GetVisualChild(int index)
    {
        return contentPresenter;
    }

    protected override Size MeasureOverride(Size constraint)
    {
        contentPresenter.Measure(Control.RenderSize);

        return Control.RenderSize;
    }
}