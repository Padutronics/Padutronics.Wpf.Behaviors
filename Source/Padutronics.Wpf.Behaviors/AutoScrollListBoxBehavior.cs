using Microsoft.Xaml.Behaviors;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace Padutronics.Wpf.Behaviors;

public sealed class AutoScrollListBoxBehavior : Behavior<ListBox>
{
    private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (AssociatedObject is ListBox listBox)
        {
            if (listBox.Items.Count > 0)
            {
                listBox.ScrollIntoView(listBox.Items[^1]);
            }
        }
    }

    protected override void OnAttached()
    {
        if (AssociatedObject is ListBox listBox)
        {
            if (listBox.Items is INotifyCollectionChanged items)
            {
                items.CollectionChanged += Items_CollectionChanged;
            }
        }

        base.OnAttached();
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (AssociatedObject is ListBox listBox)
        {
            if (listBox.Items is INotifyCollectionChanged items)
            {
                items.CollectionChanged -= Items_CollectionChanged;
            }
        }
    }
}