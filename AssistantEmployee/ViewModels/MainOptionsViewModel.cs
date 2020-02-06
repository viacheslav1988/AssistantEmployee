using AssistantEmployee.ViewModels.Workspaces;
using AssistantEmployee.ViewModels.Workspaces.PositiveResponse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace AssistantEmployee.ViewModels
{
    class MainOptionsViewModel : ViewModelBase
    {

        #region Поля

        private ObservableCollection<WorkspaceOptionsBase> workspaceOptions;

        private ICommand closingCommand;

        private ICommand okCommand;

        private ICommand cancelCommand;

        private ICommand previousCommand;

        private ICommand nextCommand;

        private ICommand resetCommand;

        #endregion

        #region Свойства

        public ObservableCollection<WorkspaceOptionsBase> WorkspaceOptions
        {
            get
            {
                if (workspaceOptions == null) workspaceOptions = new ObservableCollection<WorkspaceOptionsBase>()
                {
                    new PositiveResponseOptionsViewModel()
                };
                return workspaceOptions;
            }
            set { workspaceOptions = value; }
        }

        #endregion

        #region Команды

        //Команда: закрыть окно
        public ICommand ClosingCommand
        {
            get
            {
                if (closingCommand == null) closingCommand = new RelayCommand((parameter) =>
                {
                    foreach (WorkspaceOptionsBase options in WorkspaceOptions) options.Cancel();
                });
                return closingCommand;
            }
            set { closingCommand = value; }
        }

        //Команда: ок
        public ICommand OkCommand
        {
            get
            {
                if (okCommand == null) okCommand = new RelayCommand((parameter) =>
                {
                    foreach (WorkspaceOptionsBase options in WorkspaceOptions) options.Save();
                    (parameter as Window)?.Close();
                });
                return okCommand;
            }
            set { okCommand = value; }
        }

        //Команда: отмена
        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null) cancelCommand = new RelayCommand((parameter) => (parameter as Window)?.Close());
                return cancelCommand;
            }
            set { cancelCommand = value; }
        }

        //Команда: назад
        public ICommand PreviousCommand
        {
            get
            {
                if (previousCommand == null) previousCommand = new RelayCommand((parameter) =>
                {
                    ICollectionView view = CollectionViewSource.GetDefaultView(WorkspaceOptions);
                    if (view != null)
                    {
                        view.MoveCurrentToPrevious();
                        if (view.IsCurrentBeforeFirst) view.MoveCurrentToLast();
                    }
                });
                return previousCommand;
            }
            set { previousCommand = value; }
        }

        //Команда: вперед
        public ICommand NextCommand
        {
            get
            {
                if (nextCommand == null) nextCommand = new RelayCommand((parameter) =>
                {
                    ICollectionView view = CollectionViewSource.GetDefaultView(WorkspaceOptions);
                    if (view != null)
                    {
                        view.MoveCurrentToNext();
                        if (view.IsCurrentAfterLast) view.MoveCurrentToFirst();
                    }
                });
                return nextCommand;
            }
            set { nextCommand = value; }
        }

        //Команда: сброс
        public ICommand ResetCommand
        {
            get
            {
                if (resetCommand == null) resetCommand = new RelayCommand((parameter)
                    => (CollectionViewSource.GetDefaultView(WorkspaceOptions)?.CurrentItem as WorkspaceOptionsBase)?.Reset());
                return resetCommand;
            }
            set { resetCommand = value; }
        }

        #endregion

    }
}