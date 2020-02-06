using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AssistantEmployee.ViewModels.Workspaces.DocumentsSelection
{
    class DocumentsSelectionViewModel : WorkspaceBase
    {

        #region Поля

        private ICommand createCommand;

        #endregion

        #region События

        public event EventHandler<DataEventArgs> RequestCreate;

        #endregion

        #region Команды

        public ICommand CreateCommand
        {
            get
            {
                if (createCommand == null) createCommand = new RelayCommand((parameter) =>
                {
                    OnRequestCreate(new DataEventArgs(parameter));
                    OnRequestClose(EventArgs.Empty);
                });
                return createCommand;
            }
            set { createCommand = value; }
        }

        #endregion

        #region Методы

        protected virtual void OnRequestCreate(DataEventArgs e) => RequestCreate?.Invoke(this, e);

        #endregion

    }
}