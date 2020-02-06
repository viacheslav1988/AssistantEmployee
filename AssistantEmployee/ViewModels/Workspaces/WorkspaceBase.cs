using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AssistantEmployee.ViewModels.Workspaces
{
    abstract class WorkspaceBase : ViewModelBase
    {

        #region Поля

        private string nameWorkspace;

        private ICommand closeCommand;

        #endregion

        #region Свойства

        public string NameWorkspace
        {
            set
            {
                if (nameWorkspace == value) return;
                nameWorkspace = value;
                AutofillOnPropertyChanged();
            }
            get { return nameWorkspace; }
        }

        #endregion

        #region События

        public event EventHandler RequestClose;

        #endregion

        #region Команды

        public ICommand CloseCommand
        {
            get
            {
                if (closeCommand == null) closeCommand = new RelayCommand((parameter) => OnRequestClose(EventArgs.Empty));
                return closeCommand;
            }
            set { closeCommand = value; }
        }

        #endregion

        #region Методы

        public virtual void OnRequestClose(EventArgs e) => RequestClose?.Invoke(this, e);

        #endregion

    }
}