using AssistantEmployee.ViewModels.Workspaces;
using AssistantEmployee.ViewModels.Workspaces.DocumentsSelection;
using AssistantEmployee.ViewModels.Workspaces.PositiveResponse;
using AssistantEmployee.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace AssistantEmployee.ViewModels
{
    class MainViewModel : ViewModelBase
    {

        #region Поля

        private ObservableCollection<WorkspaceBase> workspaces;

        private ICommand openDocumentsSelectionCommand;

        private ICommand clearCurrentDocument;

        private ICommand exitApplicationCommand;

        private ICommand openOptionsWindowCommand;

        private ICommand openHelpWindowCommand;

        private ICommand openAboutWindowCommand;

        private bool wasOpenOptionsWindow;

        private bool wasOpenHelpWindow;

        private bool wasOpenAboutWindow;

        private MainOptionsView optionsWindow;

        private MainHelpView helpWindow;

        private MainAboutView aboutWindow;

        #endregion

        #region Конструкторы

        public MainViewModel()
        {
            wasOpenOptionsWindow = false;
            wasOpenHelpWindow = false;
            wasOpenAboutWindow = false;
        }

        #endregion

        #region Свойства

        public ObservableCollection<WorkspaceBase> Workspaces
        {
            get
            {
                if (workspaces == null)
                {
                    workspaces = new ObservableCollection<WorkspaceBase>();
                    IEditableCollectionView view = CollectionViewSource.GetDefaultView(workspaces) as IEditableCollectionView;
                    if (view != null) view.NewItemPlaceholderPosition = NewItemPlaceholderPosition.AtEnd;
                    workspaces.CollectionChanged += Workspaces_Changed;
                    OpenDocumentsSelectionCommand.Execute(null);
                }
                return workspaces;
            }
            set { workspaces = value; }
        }

        #endregion

        #region Команды

        //Команда: открыть панель выбора документа
        public ICommand OpenDocumentsSelectionCommand
        {
            get
            {
                if (openDocumentsSelectionCommand == null) openDocumentsSelectionCommand = new RelayCommand((parameter) =>
                {
                    DocumentsSelectionViewModel documentsSelection = Workspaces.FirstOrDefault(x => x is DocumentsSelectionViewModel) as DocumentsSelectionViewModel;
                    if (documentsSelection == null)
                    {
                        documentsSelection = new DocumentsSelectionViewModel() { NameWorkspace = "Создать документ..." };
                        Workspaces.Add(documentsSelection);
                    }
                    ActivateWorkspace(Workspaces, documentsSelection);
                });
                return openDocumentsSelectionCommand;
            }
            set { openDocumentsSelectionCommand = value; }
        }

        //Команда: Очистить текущий документ
        public ICommand ClearCurrentDocument
        {
            get
            {
                return clearCurrentDocument ?? (clearCurrentDocument = new RelayCommand((parameter) => ClearDocument(Workspaces)));
            }
            set { clearCurrentDocument = value; }
        }

        //Команда: выход из программы
        public ICommand ExitApplicationCommand
        {
            get
            {
                if (exitApplicationCommand == null) exitApplicationCommand = new RelayCommand((parameter) => Application.Current.Shutdown());
                return exitApplicationCommand;
            }
            set { exitApplicationCommand = value; }
        }

        //Команда: открыть окно настройки программы
        public ICommand OpenOptionsWindowCommand
        {
            get
            {
                if (openOptionsWindowCommand == null) openOptionsWindowCommand = new RelayCommand((parameter) =>
                {
                    if (optionsWindow == null)
                    {
                        optionsWindow = new MainOptionsView()
                        {
                            ResizeMode = ResizeMode.NoResize,
                            ShowInTaskbar = false,
                            Owner = Application.Current.MainWindow,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        };
                        optionsWindow.Activated += (sender, args) => wasOpenOptionsWindow = true;
                        optionsWindow.Closing += (sender, args) => CancelClosingWindow(sender as Window, args);
                        optionsWindow.IsVisibleChanged += (sender, args) => CenteringWindow(sender as Window, wasOpenOptionsWindow);
                    }
                    optionsWindow.ShowDialog();
                });
                return openOptionsWindowCommand;
            }
            set { openOptionsWindowCommand = value; }
        }

        //Команда: открыть окно справки
        public ICommand OpenHelpWindowCommand
        {
            get
            {
                if (openHelpWindowCommand == null) openHelpWindowCommand = new RelayCommand((parameter) =>
                {
                    if (helpWindow == null)
                    {
                        helpWindow = new MainHelpView()
                        {
                            ResizeMode = ResizeMode.CanMinimize,
                            Owner = Application.Current.MainWindow,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        };
                        helpWindow.Activated += (sender, args) => wasOpenHelpWindow = true;
                        helpWindow.Closing += (sender, args) => CancelClosingWindow(sender as Window, args);
                        helpWindow.IsVisibleChanged += (sender, args) => CenteringWindow(sender as Window, wasOpenHelpWindow);
                    }
                    helpWindow.Show();
                });
                return openHelpWindowCommand;
            }
            set { openHelpWindowCommand = value; }
        }

        //Команда: открыть окно о программе
        public ICommand OpenAboutWindowCommand
        {
            get
            {
                if (openAboutWindowCommand == null) openAboutWindowCommand = new RelayCommand((parameter) =>
                {
                    if (aboutWindow == null)
                    {
                        aboutWindow = new MainAboutView()
                        {
                            ResizeMode = ResizeMode.NoResize,
                            ShowInTaskbar = false,
                            Owner = Application.Current.MainWindow,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        };
                        aboutWindow.Activated += (sender, args) => wasOpenAboutWindow = true;
                        aboutWindow.Closing += (sender, args) => CancelClosingWindow(sender as Window, args);
                        aboutWindow.IsVisibleChanged += (sender, args) => CenteringWindow(sender as Window, wasOpenAboutWindow);
                    }
                    aboutWindow.ShowDialog();
                });
                return openAboutWindowCommand;
            }
            set { openAboutWindowCommand = value; }
        }

        #endregion

        #region Методы

        //Отслеживание изменений в коллекции
        private void Workspaces_Changed(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.NewItems != null && args.NewItems.Count != 0)
            {
                foreach (WorkspaceBase workspace in args.NewItems)
                {
                    workspace.RequestClose += CloseWorkspace;
                    DocumentsSelectionViewModel documentsSelection = workspace as DocumentsSelectionViewModel;
                    if (documentsSelection != null) documentsSelection.RequestCreate += CreateWorkspace;
                }
            }
            if (args.OldItems != null && args.OldItems.Count != 0)
            {
                foreach (WorkspaceBase workspace in args.OldItems)
                {
                    workspace.RequestClose -= CloseWorkspace;
                    DocumentsSelectionViewModel documentsSelection = workspace as DocumentsSelectionViewModel;
                    if (documentsSelection != null) documentsSelection.RequestCreate -= CreateWorkspace;
                }
            }
        }

        //Создать рабочую область
        private void CreateWorkspace(object sender, DataEventArgs args)
        {
            switch (args.Data)
            {
                case "CreatePositiveResponse":
                    {
                        PositiveResponseViewModel positiveResponse = new PositiveResponseViewModel() { NameWorkspace = "Положительный ответ" };
                        Workspaces.Add(positiveResponse);
                        ActivateWorkspace(Workspaces, positiveResponse);
                        break;
                    }
            }
        }

        //Логика закрытия рабочей области и приложения!!!!!!!!!
        private void CloseWorkspace(object sender, EventArgs args)
        {
            if (Workspaces.Count == 1 && Workspaces[0] is DocumentsSelectionViewModel) Application.Current.Shutdown();
            bool currentWorkspace = CollectionViewSource.GetDefaultView(Workspaces)?.CurrentItem == sender;
            if (Workspaces.Remove((WorkspaceBase)sender) && Workspaces.Count != 0 && currentWorkspace)
            {
                CollectionViewSource.GetDefaultView(Workspaces)?.MoveCurrentToPosition(Workspaces.Count - 1);
            }
            if (Workspaces.Count == 0) OpenDocumentsSelectionCommand.Execute(null);
        }

        //Активировать рабочую область
        private void ActivateWorkspace<T>(ObservableCollection<T> collection, T element)
            => CollectionViewSource.GetDefaultView(collection)?.MoveCurrentTo(element);

        //Скрыть окно, а не закрыть его
        private void CancelClosingWindow(Window window, CancelEventArgs arguments)
        {
            if (window == null) return;
            window.Visibility = Visibility.Hidden;
            arguments.Cancel = true;
        }

        //Центрировать окно
        private void CenteringWindow(Window window, bool wasOpen)
        {
            if (window == null && window.Owner == null) return;
            if (window.IsVisible && wasOpen)
            {
                if (window.Owner.WindowState == WindowState.Normal)
                {
                    window.Left = window.Owner.Left + (window.Owner.ActualWidth - window.ActualWidth) / 2.0;
                    window.Top = window.Owner.Top + (window.Owner.ActualHeight - window.ActualHeight) / 2.0;
                }
                else if (window.Owner.WindowState == WindowState.Maximized)
                {
                    window.Left = (SystemParameters.PrimaryScreenWidth - window.Width) / 2.0;
                    window.Top = (SystemParameters.PrimaryScreenHeight - window.Height) / 2.0;
                }
            }
        }

        //Очистить текущий документ
        private void ClearDocument<T>(T collection) where T : ObservableCollection<WorkspaceBase>
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(collection);
            switch (view?.CurrentItem)
            {
                case null: return;
                case PositiveResponseViewModel item:
                    int position = collection.IndexOf(item);
                    collection.Remove(item);
                    PositiveResponseViewModel doc = new PositiveResponseViewModel() { NameWorkspace = "Положительный ответ" };
                    collection.Add(doc);
                    collection.Move(collection.IndexOf(doc), position);
                    ActivateWorkspace(collection, doc);
                    break;
            }
        }

        #endregion

    }
}
