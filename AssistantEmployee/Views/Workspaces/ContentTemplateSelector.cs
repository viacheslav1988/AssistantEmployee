using AssistantEmployee.ViewModels.Workspaces.DocumentsSelection;
using AssistantEmployee.ViewModels.Workspaces.PositiveResponse;
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
    class ContentTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (element != null && item != null)
            {
                if (item is DocumentsSelectionViewModel) return element.FindResource("DocumentsSelectionTemplate") as DataTemplate;
                else if (item is PositiveResponseViewModel) return element.FindResource("PositiveResponseTemplate") as DataTemplate;
                else if (item == CollectionView.NewItemPlaceholder) return element.FindResource("ContentPlaceholderTemplate") as DataTemplate;
            }
            return null;
        }
    }
}