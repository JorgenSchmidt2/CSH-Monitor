using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

namespace CSH_Monitor.UI.Behaviors
{
    public class TreeViewSelectedItemBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty SelectedItemProperty =
    DependencyProperty.Register(
        nameof(SelectedItem),
        typeof(object),
        typeof(TreeViewSelectedItemBehavior),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public static readonly DependencyProperty AutoExpandProperty =
            DependencyProperty.Register(
                nameof(AutoExpand),
                typeof(bool),
                typeof(TreeViewSelectedItemBehavior),
                new PropertyMetadata(true));

        public bool AutoExpand
        {
            get => (bool)GetValue(AutoExpandProperty);
            set => SetValue(AutoExpandProperty, value);
        }

        private TreeView? TargetTreeView;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnAssociatedObjectLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= OnAssociatedObjectLoaded;

            if (TargetTreeView != null)
            {
                TargetTreeView.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
            }
        }

        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            TargetTreeView = FindTreeView(AssociatedObject);

            if (TargetTreeView != null)
            {
                TargetTreeView.SelectedItemChanged += OnTreeViewSelectedItemChanged;

                if (SelectedItem != null)
                {
                    SetSelectedItem(SelectedItem);
                }
            }
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = d as TreeViewSelectedItemBehavior;
            behavior?.SetSelectedItem(e.NewValue);
        }

        private void SetSelectedItem(object item)
        {
            if (TargetTreeView == null || item == null) return;

            TargetTreeView.Dispatcher.BeginInvoke(new Action(() =>
            {
                var treeViewItem = FindTreeViewItem(TargetTreeView, item);
                if (treeViewItem != null)
                {
                    if (AutoExpand)
                    {
                        ExpandParents(treeViewItem);
                    }

                    treeViewItem.IsSelected = true;
                    treeViewItem.BringIntoView();
                    treeViewItem.Focus();
                }
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        private static TreeView? FindTreeView(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is TreeView treeView)
                    return treeView;

                var result = FindTreeView(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        private static TreeViewItem? FindTreeViewItem(ItemsControl container, object item)
        {
            if (container == null) return null;

            try
            {
                var directContainer = container.ItemContainerGenerator.ContainerFromItem(item);
                if (directContainer is TreeViewItem treeItem)
                    return treeItem;

                for (int i = 0; i < container.Items.Count; i++)
                {
                    var subContainer = container.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                    if (subContainer != null)
                    {
                        var result = FindTreeViewItem(subContainer, item);
                        if (result != null)
                            return result;
                    }
                }
            }
            catch (Exception)
            {
                // нужно подумать что делать при ошибке
            }

            return null;
        }

        private static void ExpandParents(TreeViewItem item)
        {
            var parent = VisualTreeHelper.GetParent(item) as TreeViewItem;
            while (parent != null)
            {
                parent.IsExpanded = true;
                parent = VisualTreeHelper.GetParent(parent) as TreeViewItem;
            }
        }
    }
}