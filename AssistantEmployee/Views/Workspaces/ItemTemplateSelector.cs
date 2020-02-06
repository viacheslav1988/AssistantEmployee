using AssistantEmployee.ViewModels.Workspaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AssistantEmployee.Views.Workspaces
{
    class ItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (element != null && item != null)
            {
                if (item is WorkspaceBase) return element.FindResource("ItemWorkspacesTemplate") as DataTemplate;
                else if (item == CollectionView.NewItemPlaceholder) return element.FindResource("ItemPlaceholderTemplate") as DataTemplate;
            }
            return null;
        }
    }
}